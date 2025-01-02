import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { JSX } from "react";

import { Link } from "../../constants/nav-bar-links";
import styles from "./collapsible-nav-item.module.scss";
import Collapsible from "../Collapsible/collapsible.component";
import Error from "../../pages/Error/error.page";
import { CreateLinks } from "../../functions/Links/create-links";

type Props = {
    open: string;
    link: Link
};

function CollapsibleNavItem(props: Props): JSX.Element {
    const link: Link = props.link;

    return (
        <>
            <Collapsible>
                <Collapsible.Label>
                    <h2 className={styles.sideitem}>
                        <Collapsible.Toggle>
                            <NavLink to={link.path} key={link.path + '-icon'}>
                                <FontAwesomeIcon icon={link.icon} />
                            </NavLink>
                            <NavLink to={link.path} className={props.open === "true" ? styles.linkText : styles.linkTextClosed} key={link.path + '-text'}>
                                <h5>{link.title}</h5>
                            </NavLink>
                        </Collapsible.Toggle>
                    </h2>
                </Collapsible.Label>
                <Collapsible.Body>
                    {(() => {
                        if (link.children && link.children.length > 0) {
                            return link.children.map((child) => { return CreateLinks(child, props.open); });
                        } else {
                            return (
                                <Error>
                                    <h6 className='error-text'>Error Loading Nav Children!</h6>
                                    <p className='error-text'>The nav item {link.title} could not load its children.</p>
                                </Error>
                            );
                        }
                    })()}
                </Collapsible.Body>
            </Collapsible>
        </>
    );
}

export default CollapsibleNavItem;