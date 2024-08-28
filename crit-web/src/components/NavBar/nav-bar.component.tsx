import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";

import { Link, links } from "../../constants/nav-bar-links";

import styles from "./nav-bar.module.scss";
import CollapsibleNavItem from "../CollapsibleNavItem/collapsible-nav-item.component";

export function CreateLinks(
	link: Link,
	props: Props,
): JSX.Element {
	return (() => {
		if (link.children && link.children.length > 0) {
			return (<CollapsibleNavItem key={link.path} open={props.open} link={link} />);
		}

		return (
			<div key={link.path}>
				<h2 className={styles.sideitem}>
					<NavLink to={link.path} key={link.path + '-icon'}>
						<FontAwesomeIcon icon={link.icon} />
					</NavLink>
					<NavLink to={link.path} className={props.open === "true" ? styles.linkText : styles.linkTextClosed} key={link.path + '-text'}>
						<h5>{link.title}</h5>
					</NavLink>
				</h2>
			</div>
		);
	})();
}

type Props = {
	open: string;
	onToggleOpen: () => void;
};

function NavBar(props: Props) {
	return (
		<nav id="nav" className={props.open === "true" ? styles.sidenav : styles.sidenavClosed}>
			<FontAwesomeIcon
				className={styles.menuBtn}
				icon={props.open === "true" ? icon({ name: 'minus' }) : icon({ name: "bars" })}
				onClick={props.onToggleOpen}
			/>
			<div>
				{links.map((link) => { return CreateLinks(link, props); })}
			</div>
		</nav>
	);
}

export default NavBar;
