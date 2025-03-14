import { faChevronDown, faChevronUp } from "@fortawesome/free-solid-svg-icons";
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
                <FontAwesomeIcon icon={collapsed === true ? faChevronDown : faChevronUp} />
            </button>
        </>

    );
}

export default CollapsibleToggle;