import { Outlet } from "react-router-dom";

const Home = () => {
    return (
        <div>
            <h2>Home</h2>
            <hr />
            <Outlet />
        </div>
    );
}

export default Home;