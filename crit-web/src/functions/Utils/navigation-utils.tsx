import { useNavigate, NavigateFunction } from 'react-router-dom';

let navigateFn: NavigateFunction;

export const setUseNavigation = (navFn: NavigateFunction) => {
    navigateFn = navFn;
};

export const navigate = (path: string) => {
    if (navigateFn) {
        navigateFn(path);
    } else {
        console.warn('Navigation function not initialized yet.');
    }
};