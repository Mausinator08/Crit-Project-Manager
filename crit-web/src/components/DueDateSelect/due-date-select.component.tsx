import { JSX } from "react";

type DueDateSelectProps = {
    onDueDateSelect: (value: Date | null) => void;
    taskId: string;
    dueDate: Date | undefined;
};

function DueDateSelect(props: DueDateSelectProps): JSX.Element {
    return (
        <>
            <input
                id="dueDate"
                key={props.taskId + '-dueDate'}
                type="date"
                value={props.dueDate?.toString()}
                onChange={(e) => {
                    if (e.target.valueAsDate && e.target.valueAsDate >= new Date(Date.now())) {
                        props.onDueDateSelect(e.target.valueAsDate);
                    } else {
                        props.onDueDateSelect(null);
                    }
                }}
            />
        </>
    );
}

export default DueDateSelect;