import { LoadingSpinner } from "../../ui/LoadingSpinner";
import { ErrorDisplay } from "../../ui/ErrorDisplay";
import { useTutorials } from "../../hooks/useTutorials";
import { Outlet, useParams } from "react-router-dom";


export default function Tutorials() {

    const { tutorials, isLoading, error, refetch } = useTutorials();

    const { tutorialId } = useParams();

    if (tutorialId) {
        return <Outlet />;
    }

    if (isLoading) return <LoadingSpinner />;
    if (error) return <ErrorDisplay error={error} onRetry={refetch} />;

    return (
        <div>
            <h1 className="text-3xl font-bold mb-6">Tutorials</h1>
            <ul>
                {tutorials.map((tutorial, index) => (
                    <li key={index}>
                        <h2 className="text-xl font-bold">{tutorial.title}</h2>
                        <p>{tutorial.description}</p>
                    </li>
                ))}
            </ul>
        </div>
    );
}