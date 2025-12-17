import { JSX } from "react";

type MultiSelectLabelProps = {
	children?: React.ReactNode;
};

function MultiSelectLabel(props: MultiSelectLabelProps): JSX.Element {
	return (
		<>
			{props.children}
		</>

	);
}

export default MultiSelectLabel;