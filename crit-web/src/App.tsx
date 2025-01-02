import { JSX, useContext, useEffect, useState } from 'react';
import { Outlet } from 'react-router-dom';
import { ErrorBoundary } from "react-error-boundary";

import TitleBar from './components/TitleBar/title-bar.component';
import { ThemeContext } from "./contexts/Theme/theme-context";

import './styles/App.scss';
import ErrorFallback from './components/Error/error-fallback.component';

function App(): JSX.Element {
  const { theme, toggleTheme } = useContext(ThemeContext);

  const [open, setOpen] = useState(
    localStorage.getItem('nav-open') || 'false'
  );

  const toggleOpen: () => void = () => {
    if (open === 'false') {
      setOpen('true');
    } else {
      setOpen('false');
    }
  };

  useEffect(() => {
    localStorage.setItem('theme', theme);
    document.documentElement.setAttribute(
      "data-theme",
      theme
    );
  }, [theme]);

  useEffect(() => {
    localStorage.setItem('nav-open', open);
  }, [open]);

  return (
    <div className={`body`} data-theme={theme}>
      <ErrorBoundary FallbackComponent={ErrorFallback}>
        <TitleBar theme={theme} onToggleTheme={toggleTheme} open={open} onToggleOpen={toggleOpen} title='Crit' />
        <main className={open === 'true' ? 'main-content' : 'main-content-closed'}>
          <Outlet />
        </main>
      </ErrorBoundary>
    </div>
  );
}

export default App;
