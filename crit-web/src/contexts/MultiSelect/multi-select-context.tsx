import React, { createContext, useEffect, useState } from "react";

interface IMultiSelectContext {
	checkBoxes: boolean;
	onChange?: (id: string, isChecked: boolean) => void;
	disabled: boolean;
}

export const MultiSelectContext = createContext<IMultiSelectContext>({
	checkBoxes: true,
	onChange: (id: string, isChecked: boolean) => void {},
	disabled: false,
});

interface MultiSelectProviderProps {
	children?: React.ReactNode;
	onChange?: (selectedOptions: string[]) => void;
	disabled: boolean;
}

const MultiSelectProvider: React.FC<MultiSelectProviderProps> = ({ children, onChange, disabled }) => {
	const [multiSelectStates, setMultiSelectStates] = useState<{ checkBoxes: boolean, selectedOptions: string[], disabled: boolean }>({ checkBoxes: true, selectedOptions: [], disabled: false });

	useEffect(() => {
		setMultiSelectStates((prevState) => ({ ...prevState, disabled: disabled }));
	}, [disabled]);

	return (
		<MultiSelectContext.Provider
			value={{
				checkBoxes: multiSelectStates.checkBoxes,
				onChange: (id: string, isChecked: boolean) => {
					if (multiSelectStates.disabled) {
						return;
					}

					const nextSelectedOptions = (() => {
						const hasSelectedOptions: boolean = multiSelectStates.selectedOptions.includes(id);
						if (!hasSelectedOptions && isChecked) {
							return [...multiSelectStates.selectedOptions, id];
						}

						if (hasSelectedOptions && !isChecked) {
							return multiSelectStates.selectedOptions.filter(so => so !== id);
						}

						return multiSelectStates.selectedOptions;
					})();

					setMultiSelectStates((prevState) => ({ ...prevState, selectedOptions: nextSelectedOptions }));
					onChange && onChange(nextSelectedOptions);
				},
				disabled: multiSelectStates.disabled,
			}}
		>
			{children}
		</MultiSelectContext.Provider>
	);
};

export default MultiSelectProvider;