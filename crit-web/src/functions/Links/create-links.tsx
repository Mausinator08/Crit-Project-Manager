import { JSX } from "react";
import { Link } from "../../constants/nav-bar-links";
import CollapsibleNavItem from "../../components/CollapsibleNavItem/collapsible-nav-item.component";
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import styles from "./links.module.scss";

export function CreateLinks(
    link: Link,
    open: string,
): JSX.Element {
    return (() => {
        if (link.children && link.children.length > 0) {
            return (<CollapsibleNavItem key={link.path} open={open} link={link} />);
        }

        return (
            <div key={link.path}>
                <h2 className={styles.sideitem}>
                    <NavLink to={link.path} key={link.path + '-icon'}>
                        <FontAwesomeIcon icon={link.icon} />
                    </NavLink>
                    <NavLink to={link.path} className={open === "true" ? styles.linkText : styles.linkTextClosed} key={link.path + '-text'}>
                        <h5>{link.title}</h5>
                    </NavLink>
                </h2>
            </div>
        );
    })();
}