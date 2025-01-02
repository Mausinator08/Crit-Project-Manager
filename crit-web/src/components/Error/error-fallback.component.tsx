import { FallbackProps } from "react-error-boundary";

import Error from "../../pages/Error/error.page";

function ErrorFallback(props: FallbackProps) {
    return (
        <Error>
            <p>{props.error.message}</p>
            <button onClick={props.resetErrorBoundary}>Refresh</button>
        </Error>
    );
}

export default ErrorFallback;