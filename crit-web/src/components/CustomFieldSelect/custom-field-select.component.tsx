import { Dropdown } from "react-bootstrap";
import { JSX } from "react";
import { CustomField } from "../../models/custom-field.model";
import { CustomFieldType } from "../../models/custom-field-type.model";

type CustomFieldSelectProps = {
    customFields: CustomField[];
    onCustomFieldSelect: (value: string | null) => void;
    taskId: string;
    customField: string | undefined;
    customFieldType: CustomFieldType;
};

function CustomFieldSelect(props: CustomFieldSelectProps): JSX.Element {
    return (
        <Dropdown key={props.taskId + '-customField'} onSelect={props.onCustomFieldSelect}>
            <Dropdown.Toggle id="customField">
                {props.customFields.find(s => s.id === props.customField && s.customFieldTypeId === props.customFieldType.id)?.value ?? `${props.customFieldType.name} Not Set`}
            </Dropdown.Toggle>
            <Dropdown.Menu>
                <Dropdown.Item>{props.customFieldType.name} Not Set</Dropdown.Item>
                {props.customFields?.map<JSX.Element>(customField => (<Dropdown.Item eventKey={customField.id}>{customField.value}</Dropdown.Item>))}
            </Dropdown.Menu>
        </Dropdown>
    );
}

export default CustomFieldSelect;