import { JSX } from "react";

type CollapsibleLabelProps = {
    children?: React.ReactNode;
};

function CollapsibleLabel(props: CollapsibleLabelProps): JSX.Element {
    return (
        <>
            {props.children}
        </>

    );
}

export default CollapsibleLabel;