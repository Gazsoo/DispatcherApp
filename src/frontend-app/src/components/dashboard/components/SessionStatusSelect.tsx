import { AssignmentResponse, AssignmentStatus } from "../../../services/web-api-client";
import { Select } from "../../ui";
interface AssignmnetStatusSelectProps {
    assignment: AssignmentResponse;
    onSelectState?: (e: React.ChangeEvent<HTMLSelectElement>, assignmentId: number | undefined) => Promise<void>;
}
const AssignmnetStatusSelect = ({ assignment, onSelectState }: AssignmnetStatusSelectProps) => {


    return (
        <Select label="Assignment Status" value={assignment.status} onChange={e => onSelectState && onSelectState(e, assignment.id)}>
            <option value="Pending" selected={assignment.status === 'Pending'}>Pending</option>
            <option value="Cancelled" selected={assignment.status === 'Cancelled'}>Cancelled</option>
            <option value="InProgress" selected={assignment.status === 'InProgress'}>In Progress</option>
            <option value="Completed" selected={assignment.status === 'Completed'}>Completed</option>
        </Select>);
}
export default AssignmnetStatusSelect;