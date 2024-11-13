import { Outlet } from "react-router-dom";
import { GetEnvValues } from "../../constants/environment";
import { useState } from "react";
import { Project } from "../../models/project.model";

export interface Props {
    projectId: string;
}

function ProjectOptions(props: Props) {
    const [project, setProject] = useState<Project | undefined>();

    const getProject = async () => {

    }

    return (
        <div>
            <h2></h2>
            <hr />
            <Outlet />
        </div>
    );
}

export default ProjectOptions;