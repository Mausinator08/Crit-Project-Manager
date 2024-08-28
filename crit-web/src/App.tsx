import { useContext, useEffect, useState } from 'react';
import { Outlet } from 'react-router-dom';

import TitleBar from './components/TitleBar/title-bar.component';
import { ThemeContext } from "./contexts/theme-context";

import './styles/App.scss';

function App() {
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
      <div>
        <TitleBar theme={theme} onToggleTheme={toggleTheme} open={open} onToggleOpen={toggleOpen} title='Crit' />
        <main className={open === 'true' ? 'main-content' : 'main-content-closed'}>
          <Outlet />
        </main>
      </div>
    </div>
  );
}

export default App;
