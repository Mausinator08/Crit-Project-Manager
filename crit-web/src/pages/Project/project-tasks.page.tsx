import { Outlet, useParams } from "react-router-dom";
import { JSX, useContext, useEffect, useRef, useState } from "react";
import { Project } from '../../models/project.model';
import TaskTable from "../../components/Tasks/tasks-table.component";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { ProjectService } from "../../services/ProjectService.service";
import { UserService } from "../../services/UserService.service";
import { User } from "../../models/user.model";

import "./projects.scss";

function ProjectTasks(): JSX.Element {
    const { projectId } = useParams();
    const [project, setProject] = useState<Project | null>(null);
    const [hasFetched, setHasFetched] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [showSidePanel, setShowSidePanel] = useState<boolean>(false);
    const [users, setUsers] = useState<User[]>([]);
    const moduleContext = useRef(GetModuleContext('app'));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService('ProjectService');
    const userService: UserService = getService('UserService');

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
                setError(error.message);
            });

        project?.projectUserIds?.forEach((userId) => {
            userService.GetUserByUserId(userId).then((user) => {
                if (user) {
                    setUsers([...users, user]);
                }
            }).catch((error: Error) => {
                console.error(error);
                setError(error.message);
            });
        });
    }, [projectId]);

    return (
        <>
            <h2>{project?.name ?? '<no project name>'}</h2>
            <hr />
            {error && <p style={{ color: "red" }}>{error}</p>}
            <div className={showSidePanel ? "projects-panel split-panel" : "projects-panel"}>
                <div className="scrollable-panel-left">
                    {project && (<TaskTable
                        selectedProjectId={project.id!}
                        tasks={project?.tasks ?? []}
                        customFieldTypes={project?.customFieldTypes ?? []}
                        statuses={project?.statuses ?? []}
                        hiddenCustomFieldTypeIds={project?.hiddenCustomFieldTypeIds}
                        users={users}
                        priorities={project?.priorities} />)}
                </div>
                {showSidePanel && (
                    <div className="scrollable-panel-right">
                        <Outlet />
                    </div>
                )}
            </div>
        </>
    );
}

export default ProjectTasks;