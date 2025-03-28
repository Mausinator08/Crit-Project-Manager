import { JSX } from "react";
import { Dropdown } from "react-bootstrap";

type UserSelectProps = {
    assignedUser: string;
    onAssignedUserSelect: (value: string | null) => void;
    taskId: string;
};

function UserSelect(props: UserSelectProps): JSX.Element {
    return (
        <Dropdown key={props.taskId + '-assignedUser'} onSelect={props.onAssignedUserSelect}>
            <Dropdown.Toggle id="assignedUser" >
                {props.assignedUser}
            </Dropdown.Toggle>
            <Dropdown.Menu>
                <Dropdown.Item>Not Assigned</Dropdown.Item>
            </Dropdown.Menu>
        </Dropdown>
    );
}

export default UserSelect;