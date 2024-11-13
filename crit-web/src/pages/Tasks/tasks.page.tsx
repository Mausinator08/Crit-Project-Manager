import { Outlet } from "react-router-dom";

function Tasks() {
    return (
        <div>
            <h2>Tasks</h2>
            <hr />
            <Outlet />
        </div>
    );
}

export default Tasks;