import OperationKinoContainer from "../../components/OperationKino/OperationKinoContainer/OperationKinoContainer.tsx";
import styles from "./OperationKinoPage.module.css";

function OperationKinoPage() {
  return (
    <>
      <div className={styles.operationKinoPage}>
        <OperationKinoContainer />
      </div>
    </>
  );
}

export default OperationKinoPage;
