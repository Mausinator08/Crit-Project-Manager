import React, { useState } from "react";
import { faSquareCheck, faSquare } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { JSX, useContext } from "react";

import { MultiSelectContext } from "../../contexts/MultiSelect/multi-select-context";

import "./multi-select.scss";

type MultiSelectOptionProps = {
	children?: React.ReactNode;
	value: string;
	style?: React.CSSProperties
	isInitiallySelected?: boolean;
};

function MultiSelectOption(props: MultiSelectOptionProps): JSX.Element {
	const { checkBoxes, onChange, disabled } = useContext(MultiSelectContext);
	const [isSelected, setIsSelected] = useState<boolean>(props.isInitiallySelected ?? false);

	function setCheckBoxValue(selected: boolean) {
		if (disabled) {
			return;
		}

		setIsSelected(selected);
		onChange && onChange(props.value, selected);
	}

	return (
		<div className={'multi-select-item'} style={props.style}>
			{checkBoxes && (isSelected ?
				<FontAwesomeIcon className={'ms-2 me-2'} icon={faSquareCheck} onClick={(event) => setCheckBoxValue(false)}></FontAwesomeIcon> :
				<FontAwesomeIcon className={'ms-2 me-2'} icon={faSquare} onClick={(event) => setCheckBoxValue(true)}></FontAwesomeIcon>)}
			{props.children}
		</div>
	);
}

export default MultiSelectOption;