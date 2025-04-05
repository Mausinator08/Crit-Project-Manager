import { Dropdown } from "react-bootstrap";
import { JSX } from "react";

import { Status } from "../../models/status.model";

type StatusSelectProps = {
    statuses: Status[];
    onStatusSelect: (value: string | null) => void;
    taskId: string;
    status: string | undefined;
};

function StatusSelect(props: StatusSelectProps): JSX.Element {
    return (
        <Dropdown key={props.taskId + '-status'} onSelect={props.onStatusSelect}>
            <Dropdown.Toggle id="status">
                <div style={{
                    color: props.statuses.find(s => s.id === props.status)?.color,
                    backgroundColor: props.statuses.find(s => s.id === props.status)?.backgroundColor
                }}>{props.statuses.find(s => s.id === props.status)?.name ?? 'Status Not Set'}</div>
            </Dropdown.Toggle>
            <Dropdown.Menu>
                <Dropdown.Item>Status Not Set</Dropdown.Item>
                {props.statuses?.map<JSX.Element>(status => (<Dropdown.Item eventKey={status.id}><div style={{ color: status.color, backgroundColor: status.backgroundColor }}>{status.name}</div></Dropdown.Item>))}
            </Dropdown.Menu>
        </Dropdown>
    );
}

export default StatusSelect;