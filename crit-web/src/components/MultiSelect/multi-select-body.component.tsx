import { JSX } from "react";

import "./multi-select.scss";

type MultiSelectBodyProps = {
	children?: React.ReactNode;
};

function MultiSelectBody(props: MultiSelectBodyProps): JSX.Element {
	return (
		<div className={'multi-select-box'}>
			{props.children}
		</div>
	);
}

export default MultiSelectBody;