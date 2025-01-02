import { Dropdown } from "react-bootstrap";
import { JSX } from "react";

import { Status } from "../../models/status.model";

type StatusSelectProps = {
    statuses: Status[];
    onStatusSelect: (value: string | null) => void;
    path: string;
    status: string;
};

function StatusSelect(props: StatusSelectProps): JSX.Element {
    return (
        <Dropdown key={props.path + '-status'} onSelect={props.onStatusSelect}>
            <Dropdown.Toggle id="status" >
                {props.statuses.find(s => s.id === props.status)?.name ?? 'Status Not Set'}
            </Dropdown.Toggle>
            <Dropdown.Menu>
                <Dropdown.Item eventKey={'Status Not Set'}>Status Not Set</Dropdown.Item>
                {props.statuses?.map<JSX.Element>(status => (<Dropdown.Item eventKey={status.id}>{status.name}</Dropdown.Item>))}
            </Dropdown.Menu>
        </Dropdown>
    );
}

export default StatusSelect;