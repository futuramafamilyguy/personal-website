import styles from "./Equaliser.module.css";

interface EqualiserProps {
  isPlaying: boolean;
}

const Equaliser: React.FC<EqualiserProps> = ({ isPlaying }) => {
  return (
    <div className={`${styles.equaliser} ${isPlaying ? styles.active : ""}`}>
      <div className={styles.bar}></div>
      <div className={styles.bar}></div>
      <div className={styles.bar}></div>
      <div className={styles.bar}></div>
    </div>
  );
};

export default Equaliser;
