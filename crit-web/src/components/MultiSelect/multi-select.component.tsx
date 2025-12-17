import React, { JSX } from 'react';

import MultiSelectOption from './multi-select-option.component';
import MultiSelectBody from './multi-select-body.component';
import MultiSelectProvider from '../../contexts/MultiSelect/multi-select-context';
import MultiSelectLabel from './multi-select-label.component';

type MultiSelectProps = {
	children: React.ReactNode;
	onChange?: (selectedOptions: string[]) => void;
	disabled: boolean;
};

function MultiSelect(props: MultiSelectProps): JSX.Element {
	return (
		<MultiSelectProvider onChange={(selectedOptions: string[]) => props.onChange && props.onChange(selectedOptions)} disabled={props.disabled}>
			{props.children}
		</MultiSelectProvider>
	);
}

MultiSelect.Label = MultiSelectLabel;
MultiSelect.Option = MultiSelectOption;
MultiSelect.Body = MultiSelectBody;

export default MultiSelect;