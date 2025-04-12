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
    const [priorities, setPriorities] = useState<Priority[]>([]);
    const moduleContext = useRef(GetModuleContext("app"));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService("ProjectService");
    const userService: UserService = getService("UserService");
    const organizationService: OrganizationService = getService("OrganizationService");
    const statusService: StatusService = getService("StatusService");
    const priorityService: PriorityService = getService("PriorityService");

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
    }, [projectId]);

    useEffect(() => {
        if (!userId) {
            return;
        }

        userService.IsUserIdInRole(userId, USER_ROLES.User.value)
            .then((result) => {
                if (result) {
                    setIsViewOnly(!result.data!);
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
                        <label htmlFor="project-name">Project Name</label>
                        <input type="text" id="project-name" value={project.name} disabled={isViewOnly} />
                    </div>
                    <div>
                        <label htmlFor="project-description">Project Description</label>
                        <textarea id="project-description" value={project.description} disabled={isViewOnly} />
                    </div>
                    <div>
                        <label htmlFor="project-owning-organization">Project Owning Organization</label>
                        <Dropdown key={'project-owning-organization'} onSelect={(value) => {
                            if (value && !isViewOnly) {
                                project.owningOrganizationId = value;
                            }
                        }}>
                            <Dropdown.Toggle id="project-owning-organization" disabled={isViewOnly}>
                                {project.owningOrganizationId ?? 'Select Organization'}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                {projectOrganizations.map((org) => (
                                    <Dropdown.Item eventKey={org.id}>{org.name}</Dropdown.Item>
                                ))}
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                    <div>
                        <label htmlFor="project-owner-userid">Project Owning User</label>
                        <Dropdown key={'project-owner-userid'} onSelect={(value) => {
                            if (value && !isViewOnly) {
                                project.projectOwnerUserId = value;
                            }
                        }}>
                            <Dropdown.Toggle id="project-owner-userid" disabled={isViewOnly}>
                                {project.projectOwnerUserId ?? 'Select User'}
                            </Dropdown.Toggle>
                            <Dropdown.Menu>
                                {organizationAdminUsers.map((orgUser) => (
                                    <Dropdown.Item eventKey={orgUser.id}>{orgUser.username}</Dropdown.Item>
                                ))}
                            </Dropdown.Menu>
                        </Dropdown>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="project-admin-users">
                            <Form.Label>Project Admin Users</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.projectAdminUserIds = selectedOptions;
                            }} value={project.projectAdminUserIds}>
                                {organizationAdminUsers.map((orgUser) => (
                                    <option key={orgUser.id} value={orgUser.id}>{orgUser.username}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="project-users">
                            <Form.Label>Project Users</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.projectUserIds = selectedOptions;
                            }} value={project.projectUserIds}>
                                {organizationUsers.map((orgUser) => (
                                    <option key={orgUser.id} value={orgUser.id}>{orgUser.username}</option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="organizations">
                            <Form.Label>Project Organizations</Form.Label>
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
                        <Form.Group as={Col} controlId="statuses">
                            <Form.Label>Project Statuses</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.statuses = statuses.filter((status) => selectedOptions.includes(status.id!));
                                statuses.filter((status) => !selectedOptions.includes(status.id!)).forEach((status) => {
                                    statusService.DeleteStatus(status.id!)
                                        .then(() => {
                                            setStatuses(statuses.filter((s) => s.id !== status.id));
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setError(error.message);
                                        });
                                });
                            }} value={project.statuses.map((status) => status.id!)}>
                                {statuses.map((status) => (
                                    <option key={status.id} value={status.id}><div style={{
                                        color: status.color,
                                        backgroundColor: status.backgroundColor
                                    }}>{status.name}</div></option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                    <div>
                        <Form.Group as={Col} controlId="priorities">
                            <Form.Label>Project Priorities</Form.Label>
                            <Form.Control as="select" multiple disabled={isViewOnly} onChange={(e) => {
                                const selectedOptions = Array.from((e.target as unknown as HTMLSelectElement).selectedOptions).map((option) => option.value);
                                project.priorities = priorities.filter((priority) => selectedOptions.includes(priority.id!));
                                priorities.filter((priority) => !selectedOptions.includes(priority.id!)).forEach((priority) => {
                                    priorityService.DeletePriority(priority.id!)
                                        .then(() => {
                                            setPriorities(priorities.filter((s) => s.id !== priority.id));
                                        })
                                        .catch((error: Error) => {
                                            console.error(error.message);
                                            setError(error.message);
                                        });
                                });
                            }} value={project.priorities.map((priority) => priority.id!)}>
                                {priorities.map((priority) => (
                                    <option key={priority.id} value={priority.id}><div style={{
                                        color: priority.color,
                                        backgroundColor: priority.backgroundColor
                                    }}>{priority.name}</div></option>
                                ))}
                            </Form.Control>
                        </Form.Group>
                    </div>
                </div>
            )}
        </>
    );
}

export default ProjectOptions;