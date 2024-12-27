import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";

import { Link } from "../../constants/nav-bar-links";
import './collapsible-nav-item.scss';
import styles from "../NavBar/nav-bar.module.scss";

type Props = {
    open: string;
    link: Link
};

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

function CollapsibleNavItem(props: Props): JSX.Element {
    const link: Link = props.link;
    const [collapsed, setCollapsed] = useState<boolean>(true);

    function toggleAccordion(event: React.MouseEvent<HTMLElement>) {
        if (collapsed === true) {
            setCollapsed(false);
        } else {
            setCollapsed(true);
        }
    }

    return (
        <div>
            <h2 className={styles.sideitem} onClick={toggleAccordion}>
                <NavLink to={link.path} key={link.path + '-icon'}>
                    <FontAwesomeIcon icon={link.icon} />
                </NavLink>
                <NavLink to={link.path} className={props.open === "true" ? styles.linkText : styles.linkTextClosed} key={link.path + '-text'}>
                    <h5>{link.title}</h5>
                </NavLink>
                <button className="toggle-folding-button" onClick={toggleAccordion}><FontAwesomeIcon icon={collapsed === true ? icon({ name: 'chevron-down' }) : icon({ name: 'chevron-up' })} /></button>
            </h2>
            <div className={collapsed === true ? 'folded' : 'expanded'}>
                {(() => {
                    if (link.children && link.children.length > 0) {
                        return link.children.map((child) => { return CreateLinks(child, props.open); });
                    } else {
                        return (
                            <div>
                                <h6 className='error-text'>Error Loading Nav Children!</h6>
                                <p className='error-text'>The nav item {link.title} could not load its children.</p>
                            </div>
                        );
                    }
                })()}
            </div>
        </div>
    );
}

export default CollapsibleNavItem;