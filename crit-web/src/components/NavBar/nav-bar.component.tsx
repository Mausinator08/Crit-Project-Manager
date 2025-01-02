import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";
import { JSX } from "react";

import { links } from "../../constants/nav-bar-links";
import styles from "./nav-bar.module.scss";
import { CreateLinks } from "../../functions/Links/create-links";

type Props = {
	open: string;
	onToggleOpen: () => void;
};

function NavBar(props: Props): JSX.Element {
	return (
		<nav id="nav" className={props.open === "true" ? styles.sidenav : styles.sidenavClosed}>
			<FontAwesomeIcon
				className={styles.menuBtn}
				icon={props.open === "true" ? icon({ name: 'minus' }) : icon({ name: "bars" })}
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
		</nav>
	);
}

export default NavBar;
