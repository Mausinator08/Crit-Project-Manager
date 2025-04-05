import { Dropdown } from "react-bootstrap";
import { JSX } from "react";

import { Priority } from "../../models/priority.model";

type PrioritySelectProps = {
    priorities: Priority[];
    onPrioritySelect: (value: string | null) => void;
    taskId: string;
    priority: string | undefined;
};

function PrioritySelect(props: PrioritySelectProps): JSX.Element {
    return (
        <Dropdown key={props.taskId + '-priority'} onSelect={props.onPrioritySelect}>
            <Dropdown.Toggle id="priority">
                <div style={{
                    color: props.priorities.find(s => s.id === props.priority)?.color,
                    backgroundColor: props.priorities.find(s => s.id === props.priority)?.backgroundColor
                }}>{props.priorities.find(s => s.id === props.priority)?.name ?? 'Priority Not Set'}</div>
            </Dropdown.Toggle>
            <Dropdown.Menu>
                <Dropdown.Item>Priority Not Set</Dropdown.Item>
                {props.priorities?.map<JSX.Element>(priority => (<Dropdown.Item eventKey={priority.id}><div style={{ color: priority.color, backgroundColor: priority.backgroundColor }}>{priority.name}</div></Dropdown.Item>))}
            </Dropdown.Menu>
        </Dropdown>
    );
}

export default PrioritySelect;