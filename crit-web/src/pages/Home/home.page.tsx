import { JSX } from "react";
import { Outlet } from "react-router-dom";

function Home(): JSX.Element {
    return (
        <>
            <h2>Home</h2>
            <hr />
            <Outlet />
        </>
    );
}

export default Home;