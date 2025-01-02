import { JSX, useContext } from "react";

import './collapsible.scss';
import { CollapsibleContext } from "../../contexts/Collapsible/collapsible-context";

type CollapsibleBodyProps = {
    children?: React.ReactNode;
};

function CollapsibleBody(props: CollapsibleBodyProps): JSX.Element {
    const { collapsed } = useContext(CollapsibleContext);

    return (
        <div className={collapsed === true ? 'folded' : 'expanded'}>
            {props.children}
        </div>
    );
}

export default CollapsibleBody;