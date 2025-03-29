import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMinus, faBars } from "@fortawesome/free-solid-svg-icons";
import { JSX, useState, useEffect, useContext, useRef } from "react";

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
	const moduleContext = useRef(GetModuleContext("app"));
	const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
	const [links, setLinks] = useState<Link[]>([]);
	const { getService } = useContext(moduleContext.current.context);
	const authService: AuthService = getService(AuthService);
	const projectService: ProjectService = getService(ProjectService);
	const [isAutoRefreshEnabled, setIsAutoRefreshEnabled] = useState<boolean>(
		localStorage.getItem("isAutoRefreshEnabled") === "true"
	);
	const [autoRefreshInterval, setAutoRefreshInterval] = useState<number>(
		parseInt(localStorage.getItem("autoRefreshInterval") || "1", 10)
	);
	const [autoRefreshIntervalInstance, setAutoRefreshIntervalInstance] =
		useState<NodeJS.Timeout | null>(null);
	const [authIntervalInstance, setAuthIntervalInstance] =
		useState<NodeJS.Timeout | null>(null);
	const [hasFetched, setHasFetched] = useState<boolean>(false);
	const [projectIds, setProjectIds] = useState<string[]>([]);

	useEffect(() => {
		if (hasFetched) {
			return;
		}

		setHasFetched(true);

		if (authIntervalInstance) {
			clearInterval(authIntervalInstance);
		}

		setAuthIntervalInstance(
			setInterval(() => {
				if (authService) {
					setIsAuthenticated(authService.IsAuthenticated());
				}

				setProjectIds(projectService.GetProjectIds());
			}, 1000)
		);
	}, []);

	useEffect(() => {
		if (isAuthenticated) {
			GetLinks().then((links) => setLinks(links));
		} else {
			GetLoggedOutLinks().then((links) => setLinks(links));
		}
	}, [isAuthenticated, projectIds]);

	useEffect(() => {
		localStorage.setItem(
			"isAutoRefreshEnabled",
			isAutoRefreshEnabled.toString()
		);
		localStorage.setItem(
			"autoRefreshInterval",
			autoRefreshInterval.toString()
		);

		if (autoRefreshIntervalInstance) {
			clearInterval(autoRefreshIntervalInstance);
		}

		if (!isAutoRefreshEnabled) return;

		setAutoRefreshIntervalInstance(
			setInterval(() => {
				if (isAuthenticated) {
					GetLinks().then((links) => setLinks(links));
				} else {
					GetLoggedOutLinks().then((links) => setLinks(links));
				}
			}, autoRefreshInterval * 60000)
		); // Convert minutes to milliseconds
	}, [isAutoRefreshEnabled, autoRefreshInterval, isAuthenticated]);

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
				{links.map<JSX.Element | undefined>((link) => {
					return CreateLinks(link, props.open);
				})}
			</div>
		</nav>
	);
}

export default NavBar;
