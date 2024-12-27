import { Outlet } from "react-router-dom";

function Home(): JSX.Element {
    return (
        <div>
            <h2>Home</h2>
            <hr />
            <Outlet />
        </div>
    );
}

export default Home;