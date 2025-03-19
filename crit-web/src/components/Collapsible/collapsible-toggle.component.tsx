import { faChevronDown, faChevronUp } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { JSX, useContext } from "react";

import './collapsible.scss';
import { CollapsibleContext } from "../../contexts/Collapsible/collapsible-context";

type CollapsibleToggleProps = {
    children?: React.ReactNode;
    isOpen?: string;
};

function CollapsibleToggle(props: CollapsibleToggleProps): JSX.Element {
    const { collapsed, toggleAccordion, setCollapsed } = useContext(CollapsibleContext);

    if (props.isOpen === 'false') {
        setCollapsed(true);
    }

    return (
        <>
            {props.children}
            {props.isOpen === 'true' && (<FontAwesomeIcon className="toggle-folding-button" icon={collapsed === true ? faChevronDown : faChevronUp} onClick={toggleAccordion} />)}
        </>
    );
}

export default CollapsibleToggle;