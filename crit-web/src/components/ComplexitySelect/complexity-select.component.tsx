import { JSX } from "react";

type ComplexitySelectProps = {
    onComplexitySelect: (value: number | null) => void;
    taskId: string;
    complexity: number | undefined;
};

function ComplexitySelect(props: ComplexitySelectProps): JSX.Element {
    return (
        <>
            <input
                id="complexity"
                type="number"
                value={props.complexity}
                onChange={(e) => {
                    if (!isNaN(e.target.valueAsNumber) && e.target.valueAsNumber >= 0) {
                        props.onComplexitySelect(e.target.valueAsNumber);
                    } else {
                        props.onComplexitySelect(null);
                    }
                }}
            />
        </>
    );
}

export default ComplexitySelect;