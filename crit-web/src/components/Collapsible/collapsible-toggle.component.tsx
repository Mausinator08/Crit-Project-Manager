import { icon } from "@fortawesome/fontawesome-svg-core/import.macro";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { JSX, useContext } from "react";

import './collapsible.scss';
import { CollapsibleContext } from "../../contexts/Collapsible/collapsible-context";

type CollapsibleToggleProps = {
    children?: React.ReactNode;
};

function CollapsibleToggle(props: CollapsibleToggleProps): JSX.Element {
    const { collapsed, toggleAccordion } = useContext(CollapsibleContext);

    return (
        <>
            {props.children}
            <button className="toggle-folding-button" onClick={toggleAccordion}>
                <FontAwesomeIcon icon={collapsed === true ? icon({ name: 'chevron-down' }) : icon({ name: 'chevron-up' })} />
            </button>
        </>

    );
}

export default CollapsibleToggle;