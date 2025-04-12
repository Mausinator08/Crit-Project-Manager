import { JSX, useContext, useEffect, useState } from "react";
import { Outlet, useNavigate } from "react-router-dom";

import TitleBar from "./components/TitleBar/title-bar.component";
import { ThemeContext } from "./contexts/Theme/theme-context";

import "./styles/App.scss";
import { setUseNavigation } from "./functions/Utils/navigation-utils";
import ModuleProvider from "./contexts/Module/module-context";
import { AuthService } from "./services/AuthService.service";
import { ProjectService } from "./services/ProjectService.service";
import { UserService } from "./services/UserService.service";
import { OrganizationService } from "./services/OrganizationService";
import { StatusService } from "./services/StatusService.service";
import { PriorityService } from "./services/PriorityService.service";

function App(): JSX.Element {
	const navFn = useNavigate();

	useEffect(() => {
		setUseNavigation(navFn);
	}, [navFn]);

	const { theme, toggleTheme } = useContext(ThemeContext);

	const [open, setOpen] = useState(
		localStorage.getItem("nav-open") || "false"
	);

	const toggleOpen: () => void = () => {
		if (open === "false") {
			setOpen("true");
		} else {
			setOpen("false");
		}
	};

	useEffect(() => {
		localStorage.setItem("theme", theme);
		document.documentElement.setAttribute("data-theme", theme);
	}, [theme]);

	useEffect(() => {
		localStorage.setItem("nav-open", open);
	}, [open]);

	return (
		<div className={`body`} data-theme={theme}>
			<ModuleProvider
				services={[AuthService, ProjectService, UserService, OrganizationService, StatusService, PriorityService]}
				id="app"
				key="module_provider_app"
			>
				<TitleBar
					theme={theme}
					onToggleTheme={toggleTheme}
					open={open}
					onToggleOpen={toggleOpen}
					title="Crit"
				/>
				<main
					className={
						open === "true" ? "main-content" : "main-content-closed"
					}
				>
					<Outlet />
				</main>
			</ModuleProvider>
		</div>
	);
}

export default App;
