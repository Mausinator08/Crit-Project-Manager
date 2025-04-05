import { JSX } from "react";
import { Dropdown } from "react-bootstrap";
import { User } from "../../models/user.model";

type UserSelectProps = {
    users: User[];
    assignedUser: string | undefined;
    onAssignedUserSelect: (value: string | null) => void;
    taskId: string;
};

function UserSelect(props: UserSelectProps): JSX.Element {
    return (
        <Dropdown key={props.taskId + '-assignedUser'} onSelect={props.onAssignedUserSelect}>
            <Dropdown.Toggle id="assignedUser" >
                {props.users.find(s => s.id === props.assignedUser)?.username ?? 'Not Assigned'}
            </Dropdown.Toggle>
            <Dropdown.Menu>
                <Dropdown.Item>Not Assigned</Dropdown.Item>
                {props.users?.map<JSX.Element>(user => (<Dropdown.Item eventKey={user.id}>{user.username}</Dropdown.Item>))}
            </Dropdown.Menu>
        </Dropdown>
    );
}

export default UserSelect;