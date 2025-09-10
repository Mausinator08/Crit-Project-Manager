import { JSX, useContext, useEffect, useRef, useState } from "react";
import { useParams } from "react-router-dom";
import { Project } from "../../models/project.model";

import "./projects.scss";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { UserService } from "../../services/UserService.service";
import { ProjectService } from "../../services/ProjectService.service";
import { ApiResult } from "../../models/responses/api-result.model";
import { Col, Dropdown, Form } from "react-bootstrap";
import { OrganizationService } from "../../services/OrganizationService";
import { Organization } from "../../models/organization.model";
import { USER_ROLES } from "../../constants/user-roles";
import { User } from "../../models/user.model";
import { Status } from "../../models/status.model";
import { StatusService } from "../../services/StatusService.service";
import { PriorityService } from "../../services/PriorityService.service";
import { Priority } from "../../models/priority.model";
import { CustomFieldType } from "../../models/custom-field-type.model";
import { CustomFieldTypeService } from "../../services/CustomFieldTypeService.service";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus, faTrash } from "@fortawesome/free-solid-svg-icons";

function ProjectOptions(): JSX.Element {
    const { projectId } = useParams();
    const [project, setProject] = useState<Project | null>(null);
    const [hasFetched, setHasFetched] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [userId, setUserId] = useState<string | null>(null);
    const [projectOrganizations, setProjectOrganizations] = useState<Organization[]>([]);
    const [organizationAdminUsers, setOrganizationAdminUsers] = useState<User[]>([]);
    const [organizationUsers, setOrganizationUsers] = useState<User[]>([]);
    const [isViewOnly, setIsViewOnly] = useState<boolean>(false);
    const [statuses, setStatuses] = useState<Status[]>([]);
    const [statusColor, setStatusColor] = useState<string>("#FFFFFF");
    const [statusBackgroundColor, setStatusBackgroundColor] = useState<string>("#000000");
    const [priorityColor, setPriorityColor] = useState<string>("#FFFFFF");
    const [priorityBackgroundColor, setPriorityBackgroundColor] = useState<string>("#000000");
    const [priorities, setPriorities] = useState<Priority[]>([]);
    const [customFieldTypes, setCustomFieldTypes] = useState<CustomFieldType[]>([]);
    const [hiddenCustomFieldTypeIds, setHiddenCustomFieldTypeIds] = useState<string[]>([]);
    const moduleContext = useRef(GetModuleContext("app"));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService(ProjectService);
    const userService: UserService = getService(UserService);
    const organizationService: OrganizationService = getService(OrganizationService);
    const statusService: StatusService = getService(StatusService);
    const priorityService: PriorityService = getService(PriorityService);
    const customFieldTypeService: CustomFieldTypeService = getService(CustomFieldTypeService);

    useEffect(() => {
        if (hasFetched) {
            return;
        }

        setHasFetched(true);

        if (!projectId) {
            return;
        }

        projectService.GetProject(projectId)
            .then(value => {
                setProject(value);
            })
            .catch((error: Error) => {
                console.error(error);
                setProject(null);
                setError(error.message);
            });

        userService.GetLoggedInUserId()
            .then((result) => {
                if (result) {
                    if (!result.data) {
                        console.error("No user ID found in the result.");
                        setError("No user ID found in the result.");
                        setUserId(null);
                        return;
                    }

                    setUserId(result.data);
                }
            })
            .catch((error: ApiResult<string | null>) => {
                console.error(error.message);
                (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                    console.error(err);
                });
                setError(error.message);
            });

        statusService.GetAllStatuses(projectId)
            .then((result) => {
                setStatuses(result);
            })
            .catch((error: Error) => {
                console.error(error.message);
                setError(error.message);
            });

        priorityService.GetAllPriorities(projectId)
            .then((result) => {
                setPriorities(result);
            })
            .catch((error: Error) => {
                console.error(error.message);
                setError(error.message);
            });

        customFieldTypeService.GetAllCustomFieldTypes(projectId)
            .then((result) => {
                setCustomFieldTypes(result);
            })
            .catch((error: Error) => {
                console.error(error.message);
                setError(error.message);
            });
    }, [projectId]);

    useEffect(() => {
        if (!userId) {
            return;
        }

        userService.IsUserIdInRole(userId, USER_ROLES.User.value)
            .then((result) => {
                if (result) {
                    setIsViewOnly(result.data!);
                    return;
                }

                setIsViewOnly(true);
                console.error("Could not determine user role.");
                setError("Could not determine user role.");
            })
            .catch((error: ApiResult<string | null>) => {
                console.error(error.message);
                (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                    console.error(err);
                });
                setError(error.message);
            });

        organizationService.GetAllOrganizationsForUserId(userId)
            .then((result) => {
                setProjectOrganizations(result);
            })
            .catch((error: Error) => {
                console.error(error.message);
                setError(error.message);
            });
    }, [userId]);

    useEffect(() => {
        if (!project?.owningOrganizationId) {
            return;
        }

        organizationService.GetOrganization(project.owningOrganizationId)
            .then((result) => {
                result.memberUserIds.forEach((userId) => {
                    userService.GetUserByUserId(userId)
                        .then((userResult) => {
                            setOrganizationUsers([...organizationUsers, userResult.data!]);
                        })
                        .catch((error: ApiResult<string | null>) => {
                            console.error(error.message);
                            (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                                console.error(err);
                            });
                            setError(error.message);
                        });
                });

                result.adminUserIds.forEach((adminUserId) => {
                    userService.GetUserByUserId(adminUserId)
                        .then((userResult) => {
                            setOrganizationAdminUsers([...organizationAdminUsers, userResult.data!]);
                        })
                        .catch((error: ApiResult<string | null>) => {
                            console.error(error.message);
                            (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                                console.error(err);
                            });
                            setError(error.message);
                        });
                });
            })
            .catch((error: Error) => {
                console.error(error.message);
                setError(error.message);
            });
    }, [project?.owningOrganizationId]);

    return (
        <>
            <h2>{project?.name ?? '<no project name>'}</h2>
            <h3>Options</h3>
            <hr />
            {error && <p style={{ color: "red" }}>{error}</p>}
            {project && (
                <div>
                    <div>
                        <label htmlFor="project-name">Name</label>
                        <input type="text" id="project-name" defaultValue={project.name} disabled={isViewOnly} onChange={(e) => {
                            if (e.target.value && !isViewOnly) {
                                project.name = e.target.value;
                            }
                        }} />
                    </div>
                    <div>
                        <label htmlFor="project-description">Description</label>
                        <br />
                        <textarea id="project-description" className="project-description" defaultValue={project.description} disabled={isViewOnly} onChange={(e) => {
                            if (e.target.value && !isViewOnly) {
                                project.description = e.target.value;
                            }
                        }} />
                    </div>
                    <div>
                        <label htmlFor="project-owning-organization">Owning Organization</label>
                        <Dropdown key={'project-owning-organization'} onSelect={(value) => {
                            if (value && !isViewOnly) {
                                project.owningOrganizationId = value;
                            }
                        }}>
                            <Dropdown.Toggle id="project-owning-organization" disabled={isViewOnly}>
                                {projectOrganizations.find(o => o.id === project.owningOrganizationId)?.name ?? 'Select Organization'}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                {projectOrganizations.map((org) => (
                                    <Dropdown.Item key={`organization-${org.id}`} eventKey={org.id}>{org.name}</Dropdown.Item>
                                ))}
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                    <div>
                        <label htmlFor="project-owner-userid">Owning User</label>
                        <Dropdown key={'project-owner-userid'} onSelect={(value) => {
                            if (value && !isViewOnly) {
                                project.projectOwnerUserId = value;
                            }
                        }}>
                            <Dropdown.Toggle id="project-owner-userid" disabled={isViewOnly}>
                                {organizationAdminUsers.find(u => u.id === project.projectOwnerUserId)?.userName ?? 'Select User'}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                {organizationAdminUsers.map((orgUser) => (
                                    <Dropdown.Item key={`organizationUser-${orgUser.id}`} eventKey={orgUser.id}>{orgUser.userName}</Dropdown.Item>
                                ))}
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="project-admin-users">
                            <Form.Label>Admin Users</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.projectAdminUserIds = selectedOptions;
                            }} value={project.projectAdminUserIds}>
                                {organizationAdminUsers.map((orgUser) => (
                                    <option key={orgUser.id} value={orgUser.id}>{orgUser.userName}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="project-users">
                            <Form.Label>Users</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.projectUserIds = selectedOptions;
                            }} value={project.projectUserIds}>
                                {organizationUsers.map((orgUser) => (
                                    <option key={orgUser.id} value={orgUser.id}>{orgUser.userName}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="organizations">
                            <Form.Label>Organizations</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.organizationIds = selectedOptions;
                            }} value={project.organizationIds}>
                                {projectOrganizations.map((org) => (
                                    <option key={org.id} value={org.id}>{org.name}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col}>
                            <Form.Label>Statuses</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                statuses.filter((status) => selectedOptions.includes(status.id!)).forEach((status) => {
                                    statusService.DeleteStatus(status.id!)
                                        .then(() => {
                                            setStatuses(statuses.filter((s) => s.id !== status.id));
                                            project.statuses = statuses;
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setError(error.message);
                                        });
                                });
                            }}>
                                {statuses.map((status) => (
                                    <option key={status.id} value={status.id} style={{
                                        color: status.color,
                                        backgroundColor: status.backgroundColor,
                                        width: "fit-content",
                                    }}><span style={{
                                        display: 'inline-block',
                                        fontFamily: 'Font Awesome 5 Free',
                                        fontWeight: '900',
                                        content: '\f1f8',
                                        fontSize: '14pt',
                                        width: '14pt',
                                        height: '14pt',
                                    }}></span>{status.name}</option>
                                ))}
                            </Form.Control>
                            <Form.Label>New Status</Form.Label>
                            <Form.Control as="input" type="color" disabled={isViewOnly} value={statusColor} onChange={(e) => {
                                if (isViewOnly) {
                                    return;
                                }

                                const newColor = e.target.value;
                                setStatusColor(newColor);
                            }} />
                            <Form.Control as="input" type="color" disabled={isViewOnly} value={statusBackgroundColor} onChange={(e) => {
                                if (isViewOnly) {
                                    return;
                                }

                                const newBackgroundColor = e.target.value;
                                setStatusBackgroundColor(newBackgroundColor);
                            }} />
                            <div className="control-row">
                                <FontAwesomeIcon icon={faPlus} onClick={() => {
                                    if (isViewOnly) {
                                        return;
                                    }

                                    const newStatus = new Status();
                                    const name = document.getElementById("statusName") as HTMLInputElement;
                                    const description = document.getElementById("statusDescription") as HTMLInputElement;
                                    newStatus.name = name?.value ?? "New Status";
                                    newStatus.description = description?.value ?? "New Status Description";
                                    newStatus.color = statusColor;
                                    newStatus.backgroundColor = statusBackgroundColor;
                                    newStatus.projectId = projectId!;
                                    statusService.CreateStatus(newStatus)
                                        .then((result) => {
                                            setStatuses([...statuses, result]);
                                            project.statuses = statuses;
                                            (document.getElementById("statusName") as HTMLInputElement).value = "New Status Name";
                                            (document.getElementById("statusDescription") as HTMLInputElement).value = "New Status Description";
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setError(error.message);
                                        });
                                }} className={isViewOnly ? "create-icon-disabled" : "create-icon"} />
                                <Form.Control id="statusName" as="input" type="text" placeholder="New Status Name" disabled={isViewOnly} defaultValue={"New Status"} />
                                <Form.Control id="statusDescription" as="input" type="text" placeholder="New Status Description" disabled={isViewOnly} defaultValue={"New Status Description"} />
                            </div>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col}>
                            <Form.Label>Priorities</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                priorities.filter((priority) => selectedOptions.includes(priority.id!)).forEach((priority) => {
                                    priorityService.DeletePriority(priority.id!)
                                        .then(() => {
                                            setPriorities(priorities.filter((p) => p.id !== priority.id));
                                            project.priorities = priorities;
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setError(error.message);
                                        });
                                });
                            }} value={priorities.map((priority) => priority.id!)}>
                                {priorities.map((priority) => (
                                    <option key={priority.id} value={priority.id}><div style={{
                                        color: priority.color,
                                        backgroundColor: priority.backgroundColor
                                    }}>{priority.name}</div></option>
                                ))}
                            </Form.Control>
                            <Form.Label>New Priority</Form.Label>
                            <Form.Control as="input" type="color" disabled={isViewOnly} value={priorityColor} onChange={(e) => {
                                if (isViewOnly) {
                                    return;
                                }

                                const newColor = e.target.value;
                                setPriorityColor(newColor);
                            }} />
                            <Form.Control as="input" type="color" disabled={isViewOnly} value={priorityBackgroundColor} onChange={(e) => {
                                if (isViewOnly) {
                                    return;
                                }

                                const newBackgroundColor = e.target.value;
                                setPriorityBackgroundColor(newBackgroundColor);
                            }} />
                            <div className="control-row">
                                <FontAwesomeIcon icon={faPlus} onClick={() => {
                                    if (isViewOnly) {
                                        return;
                                    }

                                    const newPriority = new Priority();
                                    const name = document.getElementById("priorityName") as HTMLInputElement;
                                    newPriority.name = name?.value ?? "New Priority";
                                    newPriority.color = priorityColor;
                                    newPriority.backgroundColor = priorityBackgroundColor;
                                    newPriority.projectId = projectId!;
                                    priorityService.CreatePriority(newPriority)
                                        .then((result) => {
                                            setPriorities([...priorities, result]);
                                            project.priorities = priorities;
                                            (document.getElementById("priorityName") as HTMLInputElement).value = "New Priority Name";
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setError(error.message);
                                        });
                                }} className={isViewOnly ? "create-icon-disabled" : "create-icon"} />
                                <Form.Control id="priorityName" as="input" type="text" placeholder="New Priority Name" disabled={isViewOnly} defaultValue={"New Priority"} />
                            </div>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col}>
                            <Form.Label>Custom Field Types</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.customFieldTypes = customFieldTypes.filter((customFieldType) => selectedOptions.includes(customFieldType.id!));
                                customFieldTypes.filter((customFieldType) => !selectedOptions.includes(customFieldType.id!)).forEach((customFieldType) => {
                                    customFieldTypeService.DeleteCustomFieldType(customFieldType.id!)
                                        .then(() => {
                                            setCustomFieldTypes(customFieldTypes.filter((c) => c.id !== customFieldType.id));
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setError(error.message);
                                        });
                                });
                            }} value={project.customFieldTypes.map((customFieldType) => customFieldType.id!)}>
                                {customFieldTypes.map((customFieldType) => (
                                    <option key={customFieldType.id} value={customFieldType.id}>{customFieldType.name}</option>
                                ))}
                            </Form.Control>
                            <div className="control-row">
                                <FontAwesomeIcon icon={faPlus} onClick={() => {
                                    if (isViewOnly) {
                                        return;
                                    }

                                    const newCustomFieldType = new CustomFieldType();
                                    const name = document.getElementById("customFieldTypeName") as HTMLInputElement;
                                    newCustomFieldType.name = name?.value ?? "New Custom Field Type";
                                    newCustomFieldType.projectId = projectId!;
                                    customFieldTypeService.CreateCustomFieldType(newCustomFieldType)
                                        .then((result) => {
                                            setCustomFieldTypes([...customFieldTypes, result]);
                                            (document.getElementById("customFieldTypeName") as HTMLInputElement).value = "New Custom Field Type Name";
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setError(error.message);
                                        });
                                }} className={isViewOnly ? "create-icon-disabled" : "create-icon"} />
                                <Form.Control id="customFieldTypeName" as="input" type="text" placeholder="New Custom Field Type Name" disabled={isViewOnly} defaultValue={"New Custom Field Type"} />
                            </div>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="hiddenCustomFieldTypes">
                            <Form.Label>Hidden Custom Field Types From Task Table</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.hiddenCustomFieldTypeIds = customFieldTypes.filter((customFieldType) => selectedOptions.includes(customFieldType.id!)).map((customFieldType) => customFieldType.id!);
                                setHiddenCustomFieldTypeIds(project.hiddenCustomFieldTypeIds);
                            }} value={project.customFieldTypes.map((customFieldType) => customFieldType.id!)}>
                                {customFieldTypes.map((customFieldType) => {
                                    if (hiddenCustomFieldTypeIds.includes(customFieldType.id!)) {
                                        return (
                                            <option key={customFieldType.id} value={customFieldType.id} defaultChecked={true}>{customFieldType.name}</option>
                                        );
                                    }

                                    return (
                                        <option key={customFieldType.id} value={customFieldType.id}>{customFieldType.name}</option>
                                    );
                                })}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <button type="button" onClick={() => {
                            if (isViewOnly) {
                                return;
                            }

                            projectService.UpdateProject(project!)
                                .then(() => {
                                    alert("Project updated successfully.");
                                })
                                .catch((error: Error) => {
                                    console.error(error.message);
                                    setError(error.message);
                                });
                        }} disabled={isViewOnly}>Save</button>
                    </div>
                </div >
            )
            }
        </>
    );
}

export default ProjectOptions;