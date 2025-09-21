import { Outlet, useParams } from "react-router-dom";
import { JSX, useContext, useEffect, useRef, useState } from "react";
import { Project } from '../../models/project.model';
import TaskTable from "../../components/Tasks/tasks-table.component";
import { GetModuleContext } from "../../contexts/Module/module-context";
import { ProjectService } from "../../services/ProjectService.service";
import { UserService } from "../../services/UserService.service";
import { User } from "../../models/requests/user.model";

import "./projects.scss";
import { ApiResult } from "../../models/responses/api-result.model";

function ProjectTasks(): JSX.Element {
    const { projectId } = useParams();
    const [project, setProject] = useState<Project | null>(null);
    const [hasFetched, setHasFetched] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);
    const [showSidePanel, setShowSidePanel] = useState<boolean>(false);
    const [users, setUsers] = useState<User[]>([]);
    const moduleContext = useRef(GetModuleContext('app'));
    const { getService } = useContext(moduleContext.current.context);
    const projectService: ProjectService = getService(ProjectService);
    const userService: UserService = getService(UserService);

    useEffect(() => {
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

        project?.projectUsers?.forEach((user) => {
            userService.GetUserByUserId(user.id!).then((result) => {
                if (result && result.data) {
                    setUsers([...users, result.data]);
                }
            }).catch((error: ApiResult<User | null>) => {
                console.error(error.message);
                (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                    console.error(err);
                });
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
                        hiddenCustomFieldTypes={project?.hiddenCustomFieldTypes}
                        users={users}
                        priorities={project?.priorities} />)}
                </div>
                {showSidePanel ? (
                    <div className="scrollable-panel-right">
                        <h2>Project Options</h2>
                        <hr />
                        <button className="float-right" type="button" onClick={() => setShowSidePanel(false)}>Close Project Options</button>
                        <Outlet />
                        <button className="float-right" type="button" onClick={() => setShowSidePanel(false)}>Close Project Options</button>
                    </div>
                ) : (
                    <div className="scrollable-panel-right">
                        <button className="float-right" onClick={() => setShowSidePanel(true)}>Show Project Options</button>
                    </div>
                )}
            </div>
        </>
    );
}

export default ProjectTasks;