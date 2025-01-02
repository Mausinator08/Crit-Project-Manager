import { JSX } from "react";

interface ErrorProps {
    children: React.ReactNode;
}

function Error({ children }: ErrorProps): JSX.Element {
    return (
        <>
            <h1>Error</h1>
            <hr />
            {children}
        </>
    );
}

export default Error;