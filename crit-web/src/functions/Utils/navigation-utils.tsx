import { useNavigate, NavigateFunction } from 'react-router-dom';

let navigateFn: NavigateFunction;

export const useNavigation = () => {
    const navigate = useNavigate();
    navigateFn = navigate;
    return navigate;
};

export const navigate = (path: string) => {
    if (navigateFn) {
        navigateFn(path);
    } else {
        console.warn('Navigation function not initialized yet.');
    }
};