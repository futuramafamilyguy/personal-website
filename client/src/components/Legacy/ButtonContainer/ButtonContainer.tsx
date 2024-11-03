import {
  useViewFavorite,
  useViewFavoriteUpdate,
} from "../../contexts/ViewFavoriteContext";
import CapsuleButton from "../Common/CapsuleButton/CapsuleButton";
import ActiveYearsContainer from "../Letterboxc/ActiveYearsContainer/ActiveYearsContainer";
import styles from "./ButtonContainer.module.css";

const ButtonContainer: React.FC = () => {
  const viewFavorite = useViewFavorite();
  const toggleViewFavorite = useViewFavoriteUpdate();

  return (
    <div className={styles.buttonContainer}>
      <ActiveYearsContainer />
      <CapsuleButton
        text={"Favorites of the Year"}
        onClick={() => toggleViewFavorite()}
        disabled={false}
        selected={viewFavorite}
      />
    </div>
  );
};

export default ButtonContainer;
