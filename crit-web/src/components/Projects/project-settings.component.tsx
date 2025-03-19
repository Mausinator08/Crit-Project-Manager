import { JSX } from "react";
import { Tooltip } from "react-tooltip";
import './projects.scss';

export interface ProjectSettingsProps {
    isAutoRefreshEnabled: boolean;
    setIsAutoRefreshEnabled: (enabled: boolean) => void;
    autoRefreshInterval: number;
    setAutoRefreshInterval: (interval: number) => void;
    isLockedProjectsEnabled: boolean;
    setIsLockedProjectsEnabled: (enabled: boolean) => void;
}

function ProjectSettings({
    isAutoRefreshEnabled,
    setIsAutoRefreshEnabled,
    autoRefreshInterval,
    setAutoRefreshInterval,
    isLockedProjectsEnabled,
    setIsLockedProjectsEnabled
}: ProjectSettingsProps): JSX.Element {
    return (

        <div className="settings-panel">
            <label>Settings</label>
            <hr />
            <div className="settings">
                <div className="settings-item">
                    <label htmlFor="autoRefresh" className="checkbox-label">
                        <input
                            id="autoRefresh"
                            type="checkbox"
                            checked={isAutoRefreshEnabled}
                            onChange={(e) => setIsAutoRefreshEnabled(e.target.checked)}
                        /><span className="checkbox-span">Auto Refresh</span>
                    </label>
                    <br />
                    {isAutoRefreshEnabled && (
                        <label htmlFor="interval">
                            {'Interval (minutes):'}
                            <input
                                id="interval"
                                type="number"
                                value={autoRefreshInterval}
                                onChange={(e) => {
                                    if (!isNaN(e.target.valueAsNumber) && e.target.valueAsNumber >= 1 && e.target.valueAsNumber <= 60) {
                                        setAutoRefreshInterval(e.target.valueAsNumber);
                                    } else {
                                        e.target.valueAsNumber = 1;
                                        setAutoRefreshInterval(1);
                                    }
                                }}
                                min={1}
                                max={60}
                                minLength={1}
                                maxLength={2}
                            />
                        </label>
                    )}
                </div>
                <div className="settings-item">
                    <label htmlFor="lockProjects" className="checkbox-label">
                        <input
                            id="lockProjects"
                            type="checkbox"
                            data-tooltip-id="lockProjectsTooltip"
                            data-tooltip-content="Locking projects will prevent them from accidentally being deleted or renamed. (Clicking on a project and making changes to project options or tasks will still be allowed.)"
                            checked={isLockedProjectsEnabled}
                            onChange={(e) => {
                                setIsLockedProjectsEnabled(e.target.checked);
                                localStorage.setItem('isLockProjectDeletionsEnabled', e.target.checked.toString());
                            }} /><Tooltip id="lockProjectsTooltip" /><span className="checkbox-span">Lock Projects</span>
                    </label>
                </div>
            </div>
        </div >
    );
}

export default ProjectSettings;