import DashboardOverview from '../components/dashboard/pages/DashboardOverview';
import Assignments from '../components/dashboard/pages/Assignments';
import Settings from '../components/dashboard/pages/Settings';
import Tutorials from '../components/dashboard/pages/Tutorials';
import Files from '../components/dashboard/pages/Files';
import Administration from '../components/dashboard/pages/Administrations';
import Profile from '../components/dashboard/pages/Profile';
import type { ComponentType } from 'react';
import FileDetails from '../components/dashboard/pages/FileDetails';
import TutorialDetails from '../components/dashboard/pages/TutorialDetails';

interface NavigationItem {
    path: string;
    label: string;
    component: ComponentType;
    children?: Array<{
        path: string;
        component: ComponentType;
    }>;
}

// Shared navigation configuration
export const dashboardNavigation: NavigationItem[] = [
    { path: '/dashboard', label: 'Overview', component: DashboardOverview },
    {
        path: '/dashboard/tutorials',
        label: 'Tutorials',
        component: Tutorials,
        children: [
            { path: "", component: Tutorials },
            { path: ":tutorialId", component: TutorialDetails },
        ]
    },
    { path: '/dashboard/assignments', label: 'Assignments', component: Assignments },
    {
        path: '/dashboard/files',
        label: 'Files',
        component: Files,
        children: [
            { path: "", component: Files },
            { path: ":fileId", component: FileDetails },
        ]
    },
    { path: '/dashboard/settings', label: 'Settings', component: Settings },
    { path: '/dashboard/administration', label: 'Administration', component: Administration },
    { path: '/dashboard/profile', label: 'Profile', component: Profile },
];