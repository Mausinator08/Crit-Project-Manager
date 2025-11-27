$DISTRO = Read-Host "If you have a WSL distro, please enter its name here. (Leave blank to select or create a new Ubuntu 24.04 distro.)"

$WSL_INSTALL
$INSTALL_TAR_GZ_FILE
$WSL_IMPORT
$INSTALL_LOC = "C:\WSL_Distros\Ubuntu-24.04"
$INSTALLED_DISTROS = wsl --list --quiet  | Out-String -Stream
$INSTALLED_DISTROS = $INSTALLED_DISTROS -replace "`0"

if ([string]::IsNullOrWhiteSpace($DISTRO)) {
	$DISTRO = "Ubuntu-24.04"
	Write-Host $DISTRO
	if (-not ($INSTALLED_DISTROS | Select-String -Pattern $DISTRO)) {
		$INSTALL_LOC = Read-Host "Where do you want to install the distro? (Leave blank for default location which will be $INSTALL_LOC)"

		Write-Host "Installing $DISTRO..."

		$WSL_INSTALL = Start-Process -FilePath wsl.exe -ArgumentList "--install -d $DISTRO --no-launch" -Wait -PassThru

		if ($WSL_INSTALL.ExitCode -eq 0) {
			Write-Host "$DISTRO installed successfully! Extracting distro..."
			$INSTALL_TAR_GZ_FILE = Get-ChildItem -Recurse 'C:\Program Files\WindowsApps\' | Where-Object { $_.Name -eq 'install.tar.gz' }

			if ([string]::IsNullOrWhiteSpace($INSTALL_TAR_GZ_FILE)) {
				Write-Host "Error locating distro!"
				exit
			}
			else {
				wsl --import $DISTRO "$INSTALL_LOC" "$INSTALL_TAR_GZ_FILE"
			}
		}
		else {
			Write-Host "$DISTRO installation failed!"
			Write-Host $WSL_INSTALL.ExitCode
			exit
		}
	}
 else {
		Write-Host "$DISTRO already installed!"
	}
}

Set-Location \\wsl$\$DISTRO

$USERNAME = Read-Host "Type your distro OS's user name with elevated permissions."

if ([string]::IsNullOrWhiteSpace($USERNAME)) {
	Write-Host "Error getting username!"
	exit
}

wsl -d $DISTRO -u $USERNAME -- bash -c "cd /home/$USERNAME && mkdir -p ./crit"

Copy-Item -Path "$PSScriptRoot" -Destination "\\wsl$\$DISTRO\home\$USERNAME\crit\" -Recurse -Force

wsl -d $DISTRO -u root -- bash -c "apt-get install dos2unix && \
dos2unix /home/$USERNAME/crit/database/start-docker-containers.sh && \
dos2unix /home/$USERNAME/crit/database/.postgres/postgres_certs.sh && \
dos2unix /home/$USERNAME/crit/database/.postgres/backup.sh && \
chmod +x /home/$USERNAME/crit/database/start-docker-containers.sh && \
chmod +x /home/$USERNAME/crit/database/.postgres/postgres_certs.sh && \
chmod +x /home/$USERNAME/crit/database/.postgres/backup.sh && \
/home/$USERNAME/crit/database/start-docker-containers.sh"
