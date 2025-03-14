import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMinus, faBars } from "@fortawesome/free-solid-svg-icons";
import { JSX, useState, useEffect, useContext } from 'react';

import { GetLinks, GetLoggedOutLinks, Link } from "../../constants/nav-bar-links";
import styles from "./nav-bar.module.scss";
import { CreateLinks } from "../../functions/Links/create-links";
import { CheckIsAuthenticated } from '../../functions/Auth/authentication';
import { ModuleContext } from "../../contexts/Module/module-context";
import { AuthService } from "../../services/AuthService.service";

type Props = {
	open: string;
	onToggleOpen: () => void;
};

function NavBar(props: Props): JSX.Element {
	const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
	const [links, setLinks] = useState<Link[]>([]);
	const { getService } = useContext(ModuleContext);
	const authService: AuthService = getService(AuthService);

	useEffect(() => {
		CheckIsAuthenticated().then(async (auth) => {
			if (auth.result) {
				setLinks(await GetLinks());
			} else {
				setLinks(await GetLoggedOutLinks());
			}

			setIsAuthenticated(auth.result);
		});
	}, [authService.IsAuthenticated()]);

	return (
		<nav id="nav" className={props.open === "true" ? styles.sidenav : styles.sidenavClosed}>
			<FontAwesomeIcon
				className={styles.menuBtn}
				icon={props.open === "true" ? faMinus : faBars}
				onClick={props.onToggleOpen}
			/>
			<div>
				{links.map<JSX.Element | undefined>((link) => {
					if (!link?.showInNavBar || (link?.showInNavBar && link?.showInNavBar.valueOf() === true)) {
						return CreateLinks(link, props.open);
					}

					return undefined;
				})}
			</div>
		</nav >
	);
}

export default NavBar;
