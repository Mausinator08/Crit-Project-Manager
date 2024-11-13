import { Outlet } from "react-router-dom";

function Projects() {
    return (
        <div>
            <h2>Projects</h2>
            <hr />
            <Outlet />
        </div>
    );
}

export default Projects;