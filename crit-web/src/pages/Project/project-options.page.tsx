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
import { User } from "../../models/requests/user.model";
import { Status } from "../../models/status.model";
import { StatusService } from "../../services/StatusService.service";
import { PriorityService } from "../../services/PriorityService.service";
import { Priority } from "../../models/priority.model";
import { CustomFieldType } from "../../models/custom-field-type.model";
import { CustomFieldTypeService } from "../../services/CustomFieldTypeService.service";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { ProjectAdmin } from "../../models/project-admin.model";
import { ProjectUser } from "../../models/project-user.model";
import MultiSelect from "../../components/MultiSelect/multi-select.component";

function ProjectOptions(): JSX.Element {
    const { projectId } = useParams();
    const moduleContext = useRef(GetModuleContext('app'));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService(ProjectService);
    const userService: UserService = getService(UserService);
    const organizationService: OrganizationService = getService(OrganizationService);
    const statusService: StatusService = getService(StatusService);
    const priorityService: PriorityService = getService(PriorityService);
    const customFieldTypeService: CustomFieldTypeService = getService(CustomFieldTypeService);
    const [projectOptionsStates, setProjectOptionsStates] = useState<{
        project: Project | null;
        hasFetched: boolean;
        error: string | null;
        userId: string | null;
        projectOrganizations: Organization[];
        organizationAdminUsers: User[];
        organizationUsers: User[];
        isViewOnly: boolean;
        statuses: Status[];
        deletedStatuses: string[];
        newStatuses: Status[];
        statusColor: string;
        statusBackgroundColor: string;
        priorityColor: string;
        priorityBackgroundColor: string;
        priorities: Priority[];
        deletedPriorities: string[];
        newPriorities: Priority[];
        customFieldTypes: CustomFieldType[];
        hiddenCustomFieldTypes: CustomFieldType[];
    }>({
        project: null,
        hasFetched: false,
        error: null,
        userId: null,
        projectOrganizations: [],
        organizationAdminUsers: [],
        organizationUsers: [],
        isViewOnly: false,
        statuses: [],
        deletedStatuses: [],
        newStatuses: [],
        statusColor: "#FFFFFF",
        statusBackgroundColor: "#000000",
        priorityColor: "#FFFFFF",
        priorityBackgroundColor: "#000000",
        priorities: [],
        deletedPriorities: [],
        newPriorities: [],
        customFieldTypes: [],
        hiddenCustomFieldTypes: [],
    });

    useEffect(() => {
        if (projectOptionsStates.hasFetched) {
            return;
        }

        setProjectOptionsStates(prevState => ({ ...prevState, hasFetched: true }));

        if (!projectId) {
            return;
        }

        projectService.GetProject(projectId)
            .then(value => {
                setProjectOptionsStates(prevState => ({ ...prevState, project: value }));
            })
            .catch((error: Error) => {
                console.error(error);
                setProjectOptionsStates(prevState => ({ ...prevState, project: null, error: error.message }));
            });

        userService.GetLoggedInUserId()
            .then((result) => {
                if (result) {
                    if (!result.data) {
                        console.error("No user ID found in the result.");
                        setProjectOptionsStates(prevState => ({ ...prevState, error: "No user ID found in the result.", userId: null }));
                        return;
                    }

                    setProjectOptionsStates(prevState => ({ ...prevState, userId: result.data! }));
                }
            })
            .catch((error: ApiResult<string | null>) => {
                console.error(error.message);
                (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                    console.error(err);
                });
                setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
            });

        statusService.GetAllStatuses(projectId)
            .then((result) => {
                setProjectOptionsStates(prevState => ({ ...prevState, statuses: result }));
            })
            .catch((error: Error) => {
                console.error(error.message);
                setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
            });

        priorityService.GetAllPriorities(projectId)
            .then((result) => {
                setProjectOptionsStates(prevState => ({ ...prevState, priorities: result }));
            })
            .catch((error: Error) => {
                console.error(error.message);
                setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
            });

        customFieldTypeService.GetAllCustomFieldTypes(projectId)
            .then((result) => {
                setProjectOptionsStates(prevState => ({ ...prevState, customFieldTypes: result }));
            })
            .catch((error: Error) => {
                console.error(error.message);
                setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
            });
    }, [
        projectId,
    ]);

    useEffect(() => {
        if (!projectOptionsStates.userId) {
            return;
        }

        userService.IsUserIdInRole(projectOptionsStates.userId, USER_ROLES.User.value)
            .then((result) => {
                if (result) {
                    setProjectOptionsStates(prevState => ({ ...prevState, isViewOnly: result.data! }));
                    return;
                }

                setProjectOptionsStates(prevState => ({ ...prevState, isViewOnly: true }));
                console.error("Could not determine user role.");
                setProjectOptionsStates(prevState => ({ ...prevState, error: "Could not determine user role." }));
            })
            .catch((error: ApiResult<string | null>) => {
                console.error(error.message);
                (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                    console.error(err);
                });
                setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
            });

        organizationService.GetAllOrganizationsForUserId(projectOptionsStates.userId)
            .then((result) => {
                setProjectOptionsStates(prevState => ({ ...prevState, projectOrganizations: result }));
            })
            .catch((error: Error) => {
                console.error(error.message);
                setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
            });
    }, [
        projectOptionsStates.userId,
    ]);

    useEffect(() => {
        if (!projectOptionsStates.project?.owningOrganizationId) {
            return;
        }

        organizationService.GetOrganization(projectOptionsStates.project.owningOrganizationId)
            .then((result) => {
                result.organizationMembers.forEach((member) => {
                    userService.GetUserByUserId(member.memberUserId)
                        .then((userResult) => {
                            setProjectOptionsStates(prevState => ({ ...prevState, organizationUsers: [...prevState.organizationUsers, userResult.data!] }));
                        })
                        .catch((error: ApiResult<string | null>) => {
                            console.error(error.message);
                            (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                                console.error(err);
                            });
                            setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                        });
                });

                result.organizationAdmins.forEach((admin) => {
                    userService.GetUserByUserId(admin.adminUserId)
                        .then((userResult) => {
                            setProjectOptionsStates(prevState => ({ ...prevState, organizationAdminUsers: [...prevState.organizationAdminUsers, userResult.data!] }));
                        })
                        .catch((error: ApiResult<string | null>) => {
                            console.error(error.message);
                            (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                                console.error(err);
                            });
                            setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                        });
                });
            })
            .catch((error: Error) => {
                console.error(error.message);
                setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
            });
    }, [
        projectOptionsStates.project?.owningOrganizationId,
    ]);

    return (
        <>
            {projectOptionsStates.error && <p style={{ color: "red" }}>{projectOptionsStates.error}</p>}
            {projectOptionsStates.project && (
                <div>
                    <div>
                        <label htmlFor="project-name">Name</label>
                        <input type="text" id="project-name" defaultValue={projectOptionsStates.project.name} disabled={projectOptionsStates.isViewOnly} onChange={(e) => {
                            if (e.target.value && !projectOptionsStates.isViewOnly) {
                                projectOptionsStates.project!.name = e.target.value;
                            }
                        }} />
                    </div>
                    <div>
                        <label htmlFor="project-description">Description</label>
                        <br />
                        <textarea id="project-description" className="project-description" defaultValue={projectOptionsStates.project.description} disabled={projectOptionsStates.isViewOnly} onChange={(e) => {
                            if (e.target.value && !projectOptionsStates.isViewOnly) {
                                projectOptionsStates.project!.description = e.target.value;
                            }
                        }} />
                    </div>
                    <div>
                        <label htmlFor="project-owning-organization">Owning Organization</label>
                        <Dropdown key={'project-owning-organization'} onSelect={(value) => {
                            if (value && !projectOptionsStates.isViewOnly) {
                                projectOptionsStates.project!.owningOrganizationId = value;
                            }
                        }}>
                            <Dropdown.Toggle id="project-owning-organization" disabled={projectOptionsStates.isViewOnly}>
                                {projectOptionsStates.projectOrganizations.find(o => o.id === projectOptionsStates.project?.owningOrganizationId)?.name ?? 'Select Organization'}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                {projectOptionsStates.projectOrganizations.map((org) => (
                                    <Dropdown.Item key={`organization-${org.id}`} eventKey={org.id}>{org.name}</Dropdown.Item>
                                ))}
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                    <div>
                        <label htmlFor="project-owner-userid">Owning User</label>
                        <Dropdown key={'project-owner-userid'} onSelect={(value) => {
                            if (value && !projectOptionsStates.isViewOnly) {
                                projectOptionsStates.project!.projectOwnerUserId = value;
                            }
                        }}>
                            <Dropdown.Toggle id="project-owner-userid" disabled={projectOptionsStates.isViewOnly}>
                                {projectOptionsStates.organizationAdminUsers.find(u => u.id === projectOptionsStates.project?.projectOwnerUserId)?.userName ?? 'Select User'}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                {projectOptionsStates.organizationAdminUsers.map((orgUser) => (
                                    <Dropdown.Item key={`organizationUser-${orgUser.id}`} eventKey={orgUser.id}>{orgUser.userName}</Dropdown.Item>
                                ))}
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="project-admin-users">
                            <Form.Label>Admin Users</Form.Label>
                            <Form.Control as="select" multiple disabled={projectOptionsStates.isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                projectOptionsStates.project!.projectAdmins = selectedOptions.map(so => new ProjectAdmin(projectId!, so));
                            }} value={projectOptionsStates.project.projectAdmins.map(pa => pa.adminId)}>
                                {projectOptionsStates.organizationAdminUsers.map((orgUser) => (
                                    <option key={orgUser.id} value={orgUser.id}>{orgUser.userName}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="project-users">
                            <Form.Label>Users</Form.Label>
                            <Form.Control as="select" multiple disabled={projectOptionsStates.isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                projectOptionsStates.project!.projectUsers = selectedOptions.map(so => new ProjectUser(projectId!, so));
                            }} value={projectOptionsStates.project.projectUsers.map(pu => pu.userId)}>
                                {projectOptionsStates.organizationUsers.map((orgUser) => (
                                    <option key={orgUser.id} value={orgUser.id}>{orgUser.userName}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="organizations">
                            <Form.Label>Organizations</Form.Label>
                            <Form.Control as="select" multiple disabled={projectOptionsStates.isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                projectOptionsStates.project!.organizations = selectedOptions.map(so => projectOptionsStates.projectOrganizations.find(po => po.id === so)!);
                            }} value={projectOptionsStates.project.organizations.map(o => o.id!)}>
                                {projectOptionsStates.projectOrganizations.map((org) => (
                                    <option key={org.id} value={org.id}>{org.name}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col}>
                            <MultiSelect disabled={projectOptionsStates.isViewOnly} onChange={(selectedOptions: string[]) => {
                                const deletedStatuses: Status[] = projectOptionsStates.statuses.filter((status) => !selectedOptions.includes(status.id!));
                                const newStatuses: Status[] = projectOptionsStates.newStatuses.filter((status) => !selectedOptions.includes(status.id!));
                                const statuses: Status[] = [...deletedStatuses, ...newStatuses, ...projectOptionsStates.statuses.filter((status) => !deletedStatuses.some((s) => s.id === status.id) && !newStatuses.some((s) => s.id === status.id))];
                                setProjectOptionsStates((prevState) => ({ ...prevState, deletedStatuses: deletedStatuses.map((status) => status.id!) }));
                                setProjectOptionsStates((prevState) => ({ ...prevState, newStatuses: newStatuses }));
                                setProjectOptionsStates((prevState) => ({ ...prevState, statuses: statuses }));
                                const project: Project = projectOptionsStates.project!;
                                project.statuses = statuses.filter((status) => !deletedStatuses.some((s) => s.id === status.id));
                                setProjectOptionsStates((prevState) => ({ ...prevState, project: project }));
                            }}>
                                <MultiSelect.Label>Statuses</MultiSelect.Label>
                                <MultiSelect.Body>
                                    {projectOptionsStates.statuses.sort((a, b) => a.name.localeCompare(b.name, undefined, { numeric: true, sensitivity: 'base' })).map((status) => (
                                        <MultiSelect.Option key={status.id} value={status.id!} isInitiallySelected={true} style={{
                                            color: status.color,
                                            backgroundColor: status.backgroundColor,
                                            width: "fit-content",
                                        }}><span className={'me-2'}>{status.name}</span></MultiSelect.Option>
                                    ))}
                                </MultiSelect.Body>
                            </MultiSelect>
                            <Form.Label>New Status</Form.Label>
                            <Form.Control as="input" type="color" disabled={projectOptionsStates.isViewOnly} value={projectOptionsStates.statusColor} onChange={(e) => {
                                if (projectOptionsStates.isViewOnly) {
                                    return;
                                }

                                const newColor = e.target.value;
                                setProjectOptionsStates(prevState => ({ ...prevState, statusColor: newColor }));
                            }} />
                            <Form.Control as="input" type="color" disabled={projectOptionsStates.isViewOnly} value={projectOptionsStates.statusBackgroundColor} onChange={(e) => {
                                if (projectOptionsStates.isViewOnly) {
                                    return;
                                }

                                const newBackgroundColor = e.target.value;
                                setProjectOptionsStates(prevState => ({ ...prevState, statusBackgroundColor: newBackgroundColor }));
                            }} />
                            <div className="control-row">
                                <FontAwesomeIcon icon={faPlus} onClick={() => {
                                    if (projectOptionsStates.isViewOnly) {
                                        return;
                                    }

                                    const newStatus = new Status();
                                    const name = document.getElementById("statusName") as HTMLInputElement;
                                    const description = document.getElementById("statusDescription") as HTMLInputElement;
                                    newStatus.name = name?.value ?? "New Status";
                                    newStatus.description = description?.value ?? "New Status Description";
                                    newStatus.color = projectOptionsStates.statusColor;
                                    newStatus.backgroundColor = projectOptionsStates.statusBackgroundColor;
                                    newStatus.projectId = projectId!;
                                    setProjectOptionsStates((prevState) => ({
                                        ...prevState,
                                        newStatuses: [...projectOptionsStates.newStatuses, newStatus],
                                        statuses: [...projectOptionsStates.statuses, newStatus]
                                    }));
                                    const project: Project = projectOptionsStates.project!;
                                    project.statuses = [...projectOptionsStates.statuses, newStatus];
                                    setProjectOptionsStates((prevState) => ({ ...prevState, project: project }));
                                    (document.getElementById("statusName") as HTMLInputElement).value = "New Status Name";
                                    (document.getElementById("statusDescription") as HTMLInputElement).value = "New Status Description";
                                }} className={projectOptionsStates.isViewOnly ? "create-icon-disabled" : "create-icon"} />
                                <Form.Control id="statusName" as="input" type="text" placeholder="New Status Name" disabled={projectOptionsStates.isViewOnly} defaultValue={"New Status"} />
                                <Form.Control id="statusDescription" as="input" type="text" placeholder="New Status Description" disabled={projectOptionsStates.isViewOnly} defaultValue={"New Status Description"} />
                            </div>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col}>
                            <MultiSelect disabled={projectOptionsStates.isViewOnly} onChange={(selectedOptions) => {
                                const deletedPriorities: Priority[] = projectOptionsStates.priorities.filter((priority) => !selectedOptions.includes(priority.id!));
                                const newPriorities: Priority[] = projectOptionsStates.newPriorities.filter((priority) => !selectedOptions.includes(priority.id!));
                                const priorities: Priority[] = [...deletedPriorities, ...newPriorities, ...projectOptionsStates.priorities.filter((priority) => !deletedPriorities.some((p) => p.id === priority.id) && !newPriorities.some((p) => p.id === priority.id))];
                                setProjectOptionsStates((prevState) => ({ ...prevState, deletedPriorities: deletedPriorities.map((priority) => priority.id!) }));
                                setProjectOptionsStates((prevState) => ({ ...prevState, newPriorities: newPriorities }));
                                setProjectOptionsStates((prevState) => ({ ...prevState, priorities: priorities }));
                                const project: Project = projectOptionsStates.project!;
                                project.priorities = priorities.filter((priority) => !deletedPriorities.some((p) => p.id === priority.id));
                                setProjectOptionsStates((prevState) => ({ ...prevState, project: project }));
                            }}>
                                <MultiSelect.Label>Priorities</MultiSelect.Label>
                                <MultiSelect.Body>
                                    {projectOptionsStates.priorities.sort((a, b) => a.name.localeCompare(b.name, undefined, { numeric: true, sensitivity: 'base' })).map((priority) => (
                                        <MultiSelect.Option key={priority.id} value={priority.id!} isInitiallySelected={true} style={{
                                            color: priority.color,
                                            backgroundColor: priority.backgroundColor,
                                            width: "fit-content",
                                        }}><span className={'me-2'}>{priority.name}</span></MultiSelect.Option>
                                    ))}
                                </MultiSelect.Body>
                            </MultiSelect>
                            <Form.Label>New Priority</Form.Label>
                            <Form.Control as="input" type="color" disabled={projectOptionsStates.isViewOnly} value={projectOptionsStates.priorityColor} onChange={(e) => {
                                if (projectOptionsStates.isViewOnly) {
                                    return;
                                }

                                const newColor = e.target.value;
                                setProjectOptionsStates(prevState => ({ ...prevState, priorityColor: newColor }));
                            }} />
                            <Form.Control as="input" type="color" disabled={projectOptionsStates.isViewOnly} value={projectOptionsStates.priorityBackgroundColor} onChange={(e) => {
                                if (projectOptionsStates.isViewOnly) {
                                    return;
                                }

                                const newBackgroundColor = e.target.value;
                                setProjectOptionsStates(prevState => ({ ...prevState, priorityBackgroundColor: newBackgroundColor }));
                            }} />
                            <div className="control-row">
                                <FontAwesomeIcon icon={faPlus} onClick={() => {
                                    if (projectOptionsStates.isViewOnly) {
                                        return;
                                    }

                                    const newPriority = new Priority();
                                    const name = document.getElementById("priorityName") as HTMLInputElement;
                                    newPriority.name = name?.value ?? "New Priority";
                                    newPriority.color = projectOptionsStates.priorityColor;
                                    newPriority.backgroundColor = projectOptionsStates.priorityBackgroundColor;
                                    newPriority.projectId = projectId!;
                                    setProjectOptionsStates((prevState) => ({
                                        ...prevState,
                                        newPriorities: [...projectOptionsStates.newPriorities, newPriority],
                                        priorities: [...projectOptionsStates.priorities, newPriority]
                                    }));
                                    const project: Project = projectOptionsStates.project!;
                                    project.priorities = [...projectOptionsStates.statuses, newPriority];
                                    setProjectOptionsStates((prevState) => ({ ...prevState, project: project }));
                                    (document.getElementById("priorityName") as HTMLInputElement).value = "New Priority Name";
                                    (document.getElementById("priorityDescription") as HTMLInputElement).value = "New Priority Description";
                                }} className={projectOptionsStates.isViewOnly ? "create-icon-disabled" : "create-icon"} />
                                <Form.Control id="priorityName" as="input" type="text" placeholder="New Priority Name" disabled={projectOptionsStates.isViewOnly} defaultValue={"New Priority"} />
                                <Form.Control id="priorityDescription" as="input" type="text" placeholder="New Priority Description" disabled={projectOptionsStates.isViewOnly} defaultValue={"New Priority Description"} />
                            </div>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col}>
                            <Form.Label>Custom Field Types</Form.Label>
                            <Form.Control as="select" multiple disabled={projectOptionsStates.isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                projectOptionsStates.project!.customFieldTypes = projectOptionsStates.customFieldTypes.filter((customFieldType) => selectedOptions.includes(customFieldType.id!));
                                projectOptionsStates.customFieldTypes.filter((customFieldType) => !selectedOptions.includes(customFieldType.id!)).forEach((customFieldType) => {
                                    customFieldTypeService.DeleteCustomFieldType(customFieldType.id!)
                                        .then(() => {
                                            setProjectOptionsStates(prevState => ({ ...prevState, customFieldTypes: prevState.customFieldTypes.filter((c) => c.id !== customFieldType.id) }));
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                                        });
                                });
                            }} value={projectOptionsStates.project.customFieldTypes.map((customFieldType) => customFieldType.id!)}>
                                {projectOptionsStates.customFieldTypes.map((customFieldType) => (
                                    <option key={customFieldType.id} value={customFieldType.id}>{customFieldType.name}</option>
                                ))}
                            </Form.Control>
                            <div className="control-row">
                                <FontAwesomeIcon icon={faPlus} onClick={() => {
                                    if (projectOptionsStates.isViewOnly) {
                                        return;
                                    }

                                    const newCustomFieldType = new CustomFieldType();
                                    const name = document.getElementById("customFieldTypeName") as HTMLInputElement;
                                    newCustomFieldType.name = name?.value ?? "New Custom Field Type";
                                    newCustomFieldType.projectId = projectId!;
                                    customFieldTypeService.CreateCustomFieldType(newCustomFieldType)
                                        .then((result) => {
                                            setProjectOptionsStates(prevState => ({ ...prevState, customFieldTypes: [...prevState.customFieldTypes, result] }));
                                            (document.getElementById("customFieldTypeName") as HTMLInputElement).value = "New Custom Field Type Name";
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                                        });
                                }} className={projectOptionsStates.isViewOnly ? "create-icon-disabled" : "create-icon"} />
                                <Form.Control id="customFieldTypeName" as="input" type="text" placeholder="New Custom Field Type Name" disabled={projectOptionsStates.isViewOnly} defaultValue={"New Custom Field Type"} />
                            </div>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="hiddenCustomFieldTypes">
                            <Form.Label>Hidden Custom Field Types From Task Table</Form.Label>
                            <Form.Control as="select" multiple disabled={projectOptionsStates.isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                projectOptionsStates.project!.hiddenCustomFieldTypes = projectOptionsStates.customFieldTypes.filter((customFieldType) => selectedOptions.includes(customFieldType.id!)).map((customFieldType) => customFieldType);
                                setProjectOptionsStates(prevState => ({ ...prevState, hiddenCustomFieldTypes: projectOptionsStates.project!.hiddenCustomFieldTypes }));
                            }} value={projectOptionsStates.project.customFieldTypes.map((customFieldType) => customFieldType.id!)}>
                                {projectOptionsStates.customFieldTypes.map((customFieldType) => {
                                    if (projectOptionsStates.hiddenCustomFieldTypes.includes(customFieldType)) {
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
                        <button className="float-left mt-2" type="button" onClick={() => {
                            if (projectOptionsStates.isViewOnly) {
                                return;
                            }

                            projectOptionsStates.newStatuses.forEach((newStatus) => {
                                statusService.CreateStatus(newStatus)
                                    .then((result) => {
                                        setProjectOptionsStates(prevState => ({ ...prevState, statuses: [...prevState.statuses, result] }));
                                    })
                                    .catch((error: Error) => {
                                        console.error(error.message);
                                        setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                                    });
                            });

                            projectOptionsStates.newPriorities.forEach((newPriority) => {
                                priorityService.CreatePriority(newPriority)
                                    .then((result) => {
                                        setProjectOptionsStates(prevState => ({ ...prevState, priorities: [...prevState.priorities, result] }));
                                    })
                                    .catch((error: Error) => {
                                        console.error(error.message);
                                        setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                                    });
                            });

                            projectOptionsStates.deletedStatuses.forEach((status) => {
                                statusService.DeleteStatus(status)
                                    .then(() => {
                                        setProjectOptionsStates(prevState => ({ ...prevState, statuses: prevState.statuses.filter((s) => s.id !== status) }));
                                    })
                                    .catch((error: Error) => {
                                        console.error(error.message);
                                        setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                                    });
                            });

                            projectOptionsStates.deletedPriorities.forEach((priority) => {
                                priorityService.DeletePriority(priority)
                                    .then(() => {
                                        setProjectOptionsStates(prevState => ({ ...prevState, priorities: prevState.priorities.filter((p) => p.id !== priority) }));
                                    })
                                    .catch((error: Error) => {
                                        console.error(error.message);
                                        setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                                    });
                            });

                            projectService.UpdateProject(projectOptionsStates.project!)
                                .then(() => {
                                    alert("Project updated successfully.");
                                })
                                .catch((error: Error) => {
                                    console.error(error.message);
                                    setProjectOptionsStates(prevState => ({ ...prevState, error: error.message }));
                                });
                        }} disabled={projectOptionsStates.isViewOnly}>Save</button>
                    </div>
                </div >
            )
            }
        </>
    );
}

export default ProjectOptions;