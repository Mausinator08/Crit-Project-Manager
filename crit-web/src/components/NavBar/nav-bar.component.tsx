import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMinus, faBars } from "@fortawesome/free-solid-svg-icons";
import { JSX, useState, useEffect, useRef, useContext } from "react";

import {
	GetLinks,
	GetLoggedOutLinks,
	Link,
} from "../../constants/nav-bar-links";
import styles from "./nav-bar.module.scss";
import { CreateLinks } from "../../functions/Links/create-links";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { AuthService } from "../../services/AuthService.service";
import { ProjectService } from "../../services/ProjectService.service";

type Props = {
	open: string;
	onToggleOpen: () => void;
};

function NavBar(props: Props): JSX.Element {
	const moduleContext = useRef(GetModuleContext('app'));
	const { getService } = useContext(moduleContext.current.context);
	const authService: AuthService = getService(AuthService);
	const projectService: ProjectService = getService(ProjectService);
	const [navBarStates, setNavBarStates] = useState<{
		isAuthenticated: boolean;
		links: Link[];
		isAutoRefreshEnabled: boolean;
		autoRefreshInterval: number;
		autoRefreshIntervalInstance: NodeJS.Timeout | null;
		authIntervalInstance: NodeJS.Timeout | null;
		hasFetched: boolean;
		projectIds: string[];
	}>({
		isAuthenticated: false,
		links: [],
		isAutoRefreshEnabled: localStorage.getItem("isAutoRefreshEnabled") === "true",
		autoRefreshInterval: parseInt(localStorage.getItem("autoRefreshInterval") || "1", 10),
		autoRefreshIntervalInstance: null,
		authIntervalInstance: null,
		hasFetched: false,
		projectIds: [],
	});

	useEffect(() => {
		if (navBarStates.hasFetched) {
			return;
		}

		setNavBarStates(prevState => ({ ...prevState, hasFetched: true }));

		if (navBarStates.authIntervalInstance) {
			clearInterval(navBarStates.authIntervalInstance);
		}

		const intervalId = setInterval(() => {
			if (authService) {
				setNavBarStates(prevState => ({ ...prevState, isAuthenticated: authService.IsAuthenticated() }));
			}

			setNavBarStates(prevState => ({ ...prevState, projectIds: projectService.GetProjectIds() }));
		}, 1000);

		setNavBarStates(prevState => ({ ...prevState, authIntervalInstance: intervalId }));
	}, []);

	useEffect(() => {
		if (navBarStates.isAuthenticated) {
			GetLinks().then((links) => setNavBarStates(prevState => ({ ...prevState, links })));
		} else {
			GetLoggedOutLinks().then((links) => setNavBarStates(prevState => ({ ...prevState, links })));
		}
	}, [navBarStates.isAuthenticated, navBarStates.projectIds]);

	useEffect(() => {
		localStorage.setItem(
			"isAutoRefreshEnabled",
			navBarStates.isAutoRefreshEnabled.toString()
		);
		localStorage.setItem(
			"autoRefreshInterval",
			navBarStates.autoRefreshInterval.toString()
		);

		if (navBarStates.autoRefreshIntervalInstance) {
			clearInterval(navBarStates.autoRefreshIntervalInstance);
		}

		if (!navBarStates.isAutoRefreshEnabled) return;

		const intervalId = setInterval(() => {
			if (navBarStates.isAuthenticated) {
				GetLinks().then((links) => setNavBarStates(prevState => ({ ...prevState, links })));
			} else {
				GetLoggedOutLinks().then((links) => setNavBarStates(prevState => ({ ...prevState, links })));
			}
		}, navBarStates.autoRefreshInterval * 60000);

		setNavBarStates(prevState => ({ ...prevState, autoRefreshIntervalInstance: intervalId }));
		// Convert minutes to milliseconds
	}, [
		navBarStates.isAutoRefreshEnabled,
		navBarStates.autoRefreshInterval,
		navBarStates.isAuthenticated,
	]);

	return (
		<nav
			id="nav"
			className={
				props.open === "true" ? styles.sidenav : styles.sidenavClosed
			}
		>
			<FontAwesomeIcon
				className={styles.menuBtn}
				icon={props.open === "true" ? faMinus : faBars}
				onClick={props.onToggleOpen}
			/>
			<div>
				{navBarStates.links.map<JSX.Element | undefined>((link) => {
					return CreateLinks(link, props.open);
				})}
			</div>
		</nav>
	);
}

export default NavBar;
