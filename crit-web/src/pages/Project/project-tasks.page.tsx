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
    const [projectTasksStates, setProjectTasksStates] = useState<{
        project: Project | null;
        hasFetched: boolean;
        error: string | null;
        showSidePanel: boolean;
        users: User[];
    }>({
        project: null,
        hasFetched: false,
        error: null,
        showSidePanel: false,
        users: [],
    });
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
                setProjectTasksStates(prevState => ({ ...prevState, project: value }));
            })
            .catch((error: Error) => {
                console.error(error);
                setProjectTasksStates(prevState => ({ ...prevState, error: error.message }));
            });

        projectTasksStates.project?.projectUsers?.forEach((user) => {
            userService.GetUserByUserId(user.id!).then((result) => {
                if (result && result.data) {
                    setProjectTasksStates(prevState => ({ ...prevState, users: [...prevState.users, result.data!] }));
                }
            }).catch((error: ApiResult<User | null>) => {
                console.error(error.message);
                (error.errors && error.errors.length > 0) && error.errors.forEach((err) => {
                    console.error(err);
                });
                setProjectTasksStates(prevState => ({ ...prevState, error: error.message }));
            });
        });
    }, [
        projectId,
    ]);

    return (
        <>
            <h2>{projectTasksStates.project?.name ?? '<no project name>'}</h2>
            <hr />
            {projectTasksStates.error && <p style={{ color: "red" }}>{projectTasksStates.error}</p>}
            <div className={projectTasksStates.showSidePanel ? "projects-panel split-panel" : "projects-panel"}>
                <div className="scrollable-panel-left">
                    {projectTasksStates.project && (<TaskTable
                        selectedProjectId={projectTasksStates.project.id!}
                        tasks={projectTasksStates.project?.tasks ?? []}
                        customFieldTypes={projectTasksStates.project?.customFieldTypes ?? []}
                        statuses={projectTasksStates.project?.statuses ?? []}
                        hiddenCustomFieldTypes={projectTasksStates.project?.hiddenCustomFieldTypes}
                        users={projectTasksStates.users}
                        priorities={projectTasksStates.project?.priorities} />)}
                </div>
                {projectTasksStates.showSidePanel ? (
                    <div className="scrollable-panel-right">
                        <h2>Project Options</h2>
                        <hr />
                        <button className="float-right" type="button" onClick={() => setProjectTasksStates(prevState => ({ ...prevState, showSidePanel: false }))}>Close Project Options</button>
                        <Outlet />
                        <button className="float-right" type="button" onClick={() => setProjectTasksStates(prevState => ({ ...prevState, showSidePanel: false }))}>Close Project Options</button>
                    </div>
                ) : (
                    <div className="scrollable-panel-right">
                        <button className="float-right" onClick={() => setProjectTasksStates(prevState => ({ ...prevState, showSidePanel: true }))}>Show Project Options</button>
                    </div>
                )}
            </div>
        </>
    );
}

export default ProjectTasks;