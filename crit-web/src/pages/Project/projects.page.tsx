import { JSX, useEffect, useState } from "react";
import { NavLink, Outlet } from "react-router-dom";

import { GetEnvValues } from "../../constants/environment";
import { Project } from "../../models/project.model";

async function getProjects(): Promise<Project[] | null> {
    return new Promise(async (resolve, reject): Promise<void> => {
        const response = await fetch(new URL(`${GetEnvValues()?.critApiUrl}/Project`), {
            method: 'GET',
            mode: 'cors',
            credentials: 'include',
        });

        if (response.status !== 200) {
            reject(await response.text());
            return;
        }

        const data: Project[] = await response.json() as Project[];
        resolve(data);
    });
}

function Projects(): JSX.Element {
    const [projects, setProjects] = useState<Project[] | null>([]);
    useEffect(() => {
        getProjects().then(value => {
            setProjects(value);
        });
    }, []);

    return (
        <>
            <h2>Projects</h2>
            <hr />
            {projects && projects?.length > 0 ? projects?.map<JSX.Element>(project => {
                return (
                    <NavLink to={`/Projects/${project.id}`} key={`/Projects/${project.id}`}>
                        <h5>{project.name}</h5>
                    </NavLink>
                );
            }) : (<Outlet />)}
        </>
    );
}

export default Projects;