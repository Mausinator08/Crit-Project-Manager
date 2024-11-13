import { Outlet } from "react-router-dom";

interface Props {
    children: React.ReactNode;
}

function Error({ children }: Props) {
    return (
        <div>
            <h2>Error</h2>
            <hr />
            {children}
        </div>
    );
}

export default Error;